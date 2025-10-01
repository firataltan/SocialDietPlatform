import React, { createContext, useContext, useState, useEffect } from 'react';
import type { User, LoginCredentials } from '../types';
import { authService } from '../services/authService';

interface AuthContextType {
  user: User | null;
  loading: boolean;
  login: (credentials: LoginCredentials) => Promise<void>;
  register: (
    firstName: string,
    lastName: string,
    userName: string,
    email: string,
    password: string,
    confirmPassword: string,
    dateOfBirth: string,
    role: number
  ) => Promise<void>;
  logout: () => void;
  authMessage: string | null;
  authError: string | null;
  clearAuthMessages: () => void;
  updateUser: (userData: User) => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [authMessage, setAuthMessage] = useState<string | null>(null);
  const [authError, setAuthError] = useState<string | null>(null);

  useEffect(() => {
    checkAuth();
  }, []);

  const clearAuthMessages = () => {
    setAuthMessage(null);
    setAuthError(null);
  };

  const checkAuth = async () => {
    try {
      const token = localStorage.getItem('token');
      if (token) {
        const userDataResponse = await authService.getCurrentUser();
        const authenticatedUser: User = {
          id: userDataResponse.userId,
          firstName: userDataResponse.firstName,
          lastName: userDataResponse.lastName,
          userName: userDataResponse.userName,
          email: userDataResponse.email,
          profileImage: userDataResponse.profilePictureUrl,
          createdAt: '',
          updatedAt: userDataResponse.updatedAt,
          dateOfBirth: userDataResponse.dateOfBirth,
          role: userDataResponse.role,
          bio: userDataResponse.bio,
          weight: userDataResponse.weight,
          height: userDataResponse.height,
          targetWeight: userDataResponse.targetWeight,
          token: token,
        };
        setUser(authenticatedUser);
      }
    } catch (error) {
      console.error('Auth check failed:', error);
      localStorage.removeItem('token');
      setUser(null);
    } finally {
      setLoading(false);
    }
  };

  const login = async (credentials: LoginCredentials) => {
    clearAuthMessages();
    try {
      const response = await authService.login(credentials);
      if (response.success) {
        const loggedInUser: User = {
          id: response.data.userId,
          firstName: response.data.fullName,
          lastName: '',
          userName: '',
          email: response.data.email,
          profileImage: undefined,
          createdAt: '',
          updatedAt: '',
          dateOfBirth: undefined,
          role: response.data.role,
          bio: undefined,
          weight: undefined,
          height: undefined,
          targetWeight: undefined,
          profilePictureUrl: undefined,
          token: response.data.accessToken,
        };
        setUser(loggedInUser);
        setAuthMessage(response.message || 'Giriş başarılı!');
      } else {
        setAuthError(response.message || 'Giriş başarısız. Lütfen bilgilerinizi kontrol edin.');
        throw new Error(response.message || 'Login failed according to backend response.');
      }
    } catch (error: any) {
      console.error('Login failed:', error);
      setAuthError(error.response?.data?.message || 'Giriş başarısız. Lütfen bilgilerinizi kontrol edin.');
      throw error;
    }
  };

  const register = async (
    firstName: string,
    lastName: string,
    userName: string,
    email: string,
    password: string,
    confirmPassword: string,
    dateOfBirth: string,
    role: number
  ) => {
    clearAuthMessages();
    try {
      await authService.register({
        firstName,
        lastName,
        userName,
        email,
        password,
        confirmPassword,
        dateOfBirth,
        role
      });
      setAuthMessage('Kayıt işlemi başarılı!');
    } catch (error: any) {
      console.error('Registration failed:', error);
      setAuthError(error.response?.data?.message || 'Kayıt işlemi başarısız.');
      throw error;
    }
  };

  const logout = () => {
    clearAuthMessages();
    authService.logout();
    setUser(null);
    setAuthMessage('Çıkış başarılı!');
  };

  const updateUser = (userData: User) => {
    setUser(userData);
  };

  return (
    <AuthContext.Provider value={{ user, loading, login, register, logout, authMessage, authError, clearAuthMessages, updateUser }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};