import React, { useState } from 'react';
import { socialService } from '../services/socialService';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';

const CreatePostPage: React.FC = () => {
  const [content, setContent] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const { user } = useAuth();
  const token = user?.token;
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!token) {
      setError('User not authenticated');
      return;
    }

    if (!content.trim()) {
      setError('Post content cannot be empty.');
      return;
    }

    setLoading(true);
    setError(null);
    setSuccess(null);

    try {
      const postData = {
        content: content,
        // Diğer alanlar (örn: recipeId, imageUrl) buraya eklenecek
      };

      const response = await socialService.createPost(token, postData);

      if (response.data) {
        setSuccess('Post created successfully!');
        setContent('');
        navigate('/community');
      } else {
        setError(response.message || 'Failed to create post.');
      }

    } catch (err) {
      setError('An error occurred while creating the post.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>Create New Post</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="content">Post Content:</label>
          <textarea
            id="content"
            value={content}
            onChange={(e) => setContent(e.target.value)}
            rows={4}
            cols={50}
            disabled={loading}
          />
        </div>
        <button type="submit" disabled={loading}>
          {loading ? 'Creating...' : 'Create Post'}
        </button>
      </form>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {success && <p style={{ color: 'green' }}>{success}</p>}
    </div>
  );
};

export default CreatePostPage; 