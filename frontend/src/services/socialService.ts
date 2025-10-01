import axios from 'axios';
import type { ApiResponse, PagedResult, PostDto, CommentDto } from './types.ts';

const API_URL = 'https://localhost:7061/api/v1'; // Backend API base URL

export interface ISocialService {
  getFeedPosts: (token: string, pageNumber?: number, pageSize?: number) => Promise<ApiResponse<PagedResult<PostDto>>>;
  createPost: (token: string, postData: { content: string; recipeId?: string; imageUrl?: string; }) => Promise<ApiResponse<PostDto>>;
  likePost: (token: string, postId: string) => Promise<ApiResponse<boolean>>;
  addComment: (token: string, postId: string, content: string) => Promise<ApiResponse<CommentDto>>;
  getPostComments: (token: string, postId: string) => Promise<ApiResponse<CommentDto[]>>;
  // Diğer toplulukla ilgili servis metotları buraya eklenecek
}

export const socialService: ISocialService = {
  getFeedPosts: async (token: string, pageNumber = 1, pageSize = 10): Promise<ApiResponse<PagedResult<PostDto>>> => {
    try {
      const response = await axios.get(`${API_URL}/posts/feed`, {
        params: {
          pageNumber,
          pageSize
        },
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      return {
        data: response.data.data, // Backend ApiResponse yapısına göre data.data olarak düzeltildi
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error fetching feed posts:', error);
      // Hata durumunda boş PagedResult döndürebiliriz veya hata detayını iletebiliriz
       if (axios.isAxiosError(error)) {
            return {
                data: undefined,
                status: error.response?.status || 500,
                message: error.response?.data?.message || error.message
            };
        } else {
            return {
                data: undefined,
                status: 500,
                message: 'An unexpected error occurred'
            };
        }
    }
  },

  createPost: async (token: string, postData: { content: string; recipeId?: string; imageUrl?: string; }): Promise<ApiResponse<PostDto>> => {
    try {
      const response = await axios.post(`${API_URL}/posts`, postData, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      return {
        data: response.data.data, // Backend ApiResponse yapısına göre
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error creating post:', error);
      if (axios.isAxiosError(error)) {
        return {
          data: undefined,
          status: error.response?.status || 500,
          message: error.response?.data?.message || error.message
        };
      } else {
        return {
          data: undefined,
          status: 500,
          message: 'An unexpected error occurred'
        };
      }
    }
  },

  likePost: async (token: string, postId: string): Promise<ApiResponse<boolean>> => {
    try {
      // Backend endpointi POST ve post ID'sini URL'den alıyor.
      const response = await axios.post(`${API_URL}/posts/${postId}/like`, null, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      // Backend'in ApiResponse<bool> dönmesini bekliyoruz
      return {
        data: response.data.data, // Backend ApiResponse yapısına göre
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error liking post:', error);
      if (axios.isAxiosError(error)) {
        return {
          data: undefined,
          status: error.response?.status || 500,
          message: error.response?.data?.message || error.message
        };
      } else {
        return {
          data: undefined,
          status: 500,
          message: 'An unexpected error occurred'
        };
      }
    }
  },

  addComment: async (token: string, postId: string, content: string): Promise<ApiResponse<CommentDto>> => {
    try {
      const response = await axios.post(`${API_URL}/posts/${postId}/comments`, { content }, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      return {
        data: response.data.data, // Backend ApiResponse<CommentDto> dönüyor
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error adding comment:', error);
      if (axios.isAxiosError(error)) {
        return {
          data: undefined,
          status: error.response?.status || 500,
          message: error.response?.data?.message || error.message
        };
      } else {
        return {
          data: undefined,
          status: 500,
          message: 'An unexpected error occurred'
        };
      }
    }
  },

  getPostComments: async (token: string, postId: string): Promise<ApiResponse<CommentDto[]>> => {
    try {
      const response = await axios.get(`${API_URL}/posts/${postId}/comments`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      // Backend'in ApiResponse<IEnumerable<CommentDto>> dönmesini bekliyoruz, frontendde CommentDto[] olarak ele alıyoruz
      return {
        data: response.data.data, // Backend ApiResponse yapısına göre
        status: response.status,
        message: response.data.message
      };
    } catch (error) {
      console.error('Error fetching comments:', error);
      if (axios.isAxiosError(error)) {
        return {
          data: undefined,
          status: error.response?.status || 500,
          message: error.response?.data?.message || error.message
        };
      } else {
        return {
          data: undefined,
          status: 500,
          message: 'An unexpected error occurred'
        };
      }
    }
  }
}; 