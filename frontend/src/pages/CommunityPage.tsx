import React, { useEffect, useState } from 'react';
import { socialService } from '../services/socialService';
import type { PostDto, CommentDto } from '../types';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Box,
  Card,
  CardContent,
  CardActions,
  Button,
  TextField,
  IconButton,
  Avatar,
  Divider,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Paper,
  CircularProgress,
  Alert,
  Stack,
  Chip,
} from '@mui/material';
import {
  Favorite,
  FavoriteBorder,
  Comment as CommentIcon,
  Send as SendIcon,
  Add as AddIcon,
} from '@mui/icons-material';
import { formatDistanceToNow } from 'date-fns';
import { tr } from 'date-fns/locale';

const CommunityPage: React.FC = () => {
  const [posts, setPosts] = useState<PostDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const { user } = useAuth();
  const token = user?.token;
  const navigate = useNavigate();

  const [comments, setComments] = useState<{[postId: string]: CommentDto[]}>({});
  const [commentLoading, setCommentLoading] = useState<{[postId: string]: boolean}>({});
  const [commentError, setCommentError] = useState<{[postId: string]: string | null}>({});
  const [newCommentContent, setNewCommentContent] = useState<{[postId: string]: string}>({});

  useEffect(() => {
    const fetchPosts = async () => {
      if (!token) {
        setError('User not authenticated');
        setLoading(false);
        return;
      }

      setLoading(true);
      setError(null);
      try {
        const response = await socialService.getFeedPosts(token);
        if (response.data) {
          setPosts(response.data.items);
          response.data.items.forEach((post: PostDto) => fetchCommentsForPost(post.id, token));
        } else {
          setError(response.message || 'Failed to fetch posts');
        }
      } catch (err) {
        setError('An error occurred while fetching posts');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchPosts();
  }, [token]);

  const fetchCommentsForPost = async (postId: string, token: string) => {
    if (!token) return;

    setCommentLoading(prev => ({ ...prev, [postId]: true }));
    setCommentError(prev => ({ ...prev, [postId]: null }));

    try {
      const response = await socialService.getPostComments(token, postId);
      if (response.data) {
        setComments(prev => ({ ...prev, [postId]: response.data || [] }));
      } else {
        setCommentError(prev => ({ ...prev, [postId]: response.message || 'Failed to fetch comments.' }));
      }
    } catch (err) {
       setCommentError(prev => ({ ...prev, [postId]: 'An error occurred while fetching comments.' }));
       console.error(err);
    } finally {
      setCommentLoading(prev => ({ ...prev, [postId]: false }));
    }
  };

  const handleCreatePostClick = () => {
    navigate('/community/new');
  };

  const handleLikeClick = async (postId: string) => {
    if (!token) {
      setError('User not authenticated. Please login to like posts.');
      return;
    }

    try {
      const response = await socialService.likePost(token, postId);
      if (response.data !== undefined) {
        setPosts(posts.map(post => {
          if (post.id === postId) {
            const newLikesCount = response.data ? post.likesCount + 1 : post.likesCount - 1;
            return { ...post, likesCount: newLikesCount, isLiked: response.data || false };
          }
          return post;
        }));
      } else {
        setError(response.message || 'Failed to like/unlike post.');
      }
    } catch (err) {
      setError('An error occurred while liking/unliking post.');
      console.error(err);
    }
  };

  const handleCommentSubmit = async (postId: string) => {
    if (!token) {
      setError('User not authenticated. Please login to comment.');
      return;
    }

    const content = newCommentContent[postId]?.trim() || '';
    if (!content) {
      setCommentError(prev => ({ ...prev, [postId]: 'Comment content cannot be empty.' }));
      return;
    }

    setCommentLoading(prev => ({ ...prev, [postId]: true }));
    setCommentError(prev => ({ ...prev, [postId]: null }));

    try {
      const response = await socialService.addComment(token, postId, content);
      if (response.data) {
        setComments(prev => ({ ...prev, [postId]: [...(prev[postId] || []), response.data!] }));
        setPosts(posts.map(post => {
            if (post.id === postId) {
                return { ...post, commentsCount: post.commentsCount + 1 };
            }
            return post;
        }));
        setNewCommentContent(prev => ({ ...prev, [postId]: '' }));
      } else {
         setCommentError(prev => ({ ...prev, [postId]: response.message || 'Failed to add comment.' }));
      }
    } catch (err) {
      setCommentError(prev => ({ ...prev, [postId]: 'An error occurred while adding comment.' }));
      console.error(err);
    } finally {
      setCommentLoading(prev => ({ ...prev, [postId]: false }));
    }
  };

  const handleCommentInputChange = (postId: string, value: string) => {
    setNewCommentContent(prev => ({ ...prev, [postId]: value }));
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="80vh">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Container maxWidth="md" sx={{ mt: 4 }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Box sx={{ mb: 4, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Topluluk
        </Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleCreatePostClick}
        >
          Yeni Post Oluştur
        </Button>
      </Box>

      {posts.length === 0 ? (
        <Paper sx={{ p: 3, textAlign: 'center' }}>
          <Typography variant="h6" color="text.secondary">
            Henüz hiç post yok.
          </Typography>
        </Paper>
      ) : (
        <Stack spacing={3}>
          {posts.map((post) => (
            <Card key={post.id} elevation={2}>
              <CardContent>
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                  <Avatar
                    src={post.user?.profileImageUrl}
                    alt={post.userName}
                    sx={{ mr: 2 }}
                  />
                  <Box>
                    <Typography variant="subtitle1" component="div">
                      {post.userName}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                      {formatDistanceToNow(new Date(post.createdAt), { addSuffix: true, locale: tr })}
                    </Typography>
                  </Box>
                </Box>
                
                <Typography variant="body1" sx={{ mb: 2 }}>
                  {post.content}
                </Typography>

                {post.imageUrl && (
                  <Box sx={{ mb: 2 }}>
                    <img
                      src={post.imageUrl}
                      alt="Post content"
                      style={{ maxWidth: '100%', borderRadius: '8px' }}
                    />
                  </Box>
                )}

                <Box sx={{ display: 'flex', gap: 1, mb: 2 }}>
                  <Chip
                    icon={<FavoriteBorder />}
                    label={`${post.likesCount} Beğeni`}
                    color={post.isLiked ? 'primary' : 'default'}
                    onClick={() => handleLikeClick(post.id)}
                  />
                  <Chip
                    icon={<CommentIcon />}
                    label={`${post.commentsCount} Yorum`}
                  />
                </Box>

                <Divider sx={{ my: 2 }} />

                <Box sx={{ mb: 2 }}>
                  <Typography variant="subtitle2" gutterBottom>
                    Yorumlar
                  </Typography>
                  
                  {commentLoading[post.id] ? (
                    <Box display="flex" justifyContent="center" p={2}>
                      <CircularProgress size={24} />
                    </Box>
                  ) : commentError[post.id] ? (
                    <Alert severity="error" sx={{ mb: 2 }}>
                      {commentError[post.id]}
                    </Alert>
                  ) : comments[post.id]?.length === 0 ? (
                    <Typography variant="body2" color="text.secondary">
                      Henüz yorum yok.
                    </Typography>
                  ) : (
                    <List>
                      {comments[post.id]?.map(comment => (
                        <ListItem key={comment.id} alignItems="flex-start" sx={{ px: 0 }}>
                          <ListItemAvatar>
                            <Avatar src={comment.user?.profileImageUrl} alt={comment.userName} />
                          </ListItemAvatar>
                          <ListItemText
                            primary={
                              <Typography variant="subtitle2">
                                {comment.userName}
                              </Typography>
                            }
                            secondary={
                              <Typography variant="body2" color="text.primary">
                                {comment.content}
                              </Typography>
                            }
                          />
                        </ListItem>
                      ))}
                    </List>
                  )}
                </Box>

                <Box sx={{ display: 'flex', gap: 1 }}>
                  <TextField
                    fullWidth
                    size="small"
                    placeholder="Yorum yaz..."
                    value={newCommentContent[post.id] || ''}
                    onChange={(e) => handleCommentInputChange(post.id, e.target.value)}
                    disabled={commentLoading[post.id]}
                  />
                  <IconButton
                    color="primary"
                    onClick={() => handleCommentSubmit(post.id)}
                    disabled={commentLoading[post.id]}
                  >
                    <SendIcon />
                  </IconButton>
                </Box>
              </CardContent>
            </Card>
          ))}
        </Stack>
      )}
    </Container>
  );
};

export default CommunityPage; 