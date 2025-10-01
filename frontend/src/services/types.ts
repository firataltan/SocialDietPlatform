export interface ApiResponse<T> {
  data?: T;
  status: number;
  message: string;
  success: boolean;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface PostDto {
  id: string;
  content: string;
  recipeId?: string;
  imageUrl?: string;
  userId: string;
  userName: string;
  createdAt: string;
  likesCount: number;
  commentsCount: number;
  isLiked: boolean;
  user: {
    profileImageUrl?: string;
  };
}

export interface CommentDto {
  id: string;
  content: string;
  userId: string;
  userName: string;
  postId: string;
  createdAt: string;
  user: {
    profileImageUrl?: string;
  };
} 