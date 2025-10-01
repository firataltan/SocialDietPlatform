import api from './api';
import type { LoginCredentials, UpdateProfilePayload } from '../types';

export interface RegisterData {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
  dateOfBirth: string;
  role: number;
}

export const authService = {
  login: async (credentials: LoginCredentials) => {
    const response = await api.post('/v1/auth/login', credentials);
    localStorage.setItem('token', response.data.data.accessToken);
    // Optionally store refresh token
    // Return the full response data object which includes success, message, and data fields
    return response.data; // Changed from: response.data as { data: { ... } };
  },

  register: async (data: RegisterData) => {
    const response = await api.post('/v1/auth/register', data);
    return response.data;
  },

  logout: () => {
    localStorage.removeItem('token');
    // Optionally remove refresh token
  },

  getCurrentUser: async () => {
    const response = await api.get('/v1/users/me');
    // TODO: Define UserDto type in frontend and map correctly
    return response.data.data as any; // Backend UserDto dönüyor
  },

  updateProfile: async (data: UpdateProfilePayload) => {
    const response = await api.put('/v1/users/profile', data);
    return response.data;
  },

  // Yeni fonksiyon: Profil resmi yükleme
  uploadProfileImage: async (file: File) => {
    const formData = new FormData();
    formData.append('file', file);

    const response = await api.post('/v1/users/upload-profile-image', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });

    return response.data as { url: string }; // Backend'in { url: string } döneceğini varsayıyoruz
  },
}; 