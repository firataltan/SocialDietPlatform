import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7061/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor for API calls
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor for API calls
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    // Token yenileme işlemi burada yapılabilir
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      // Token yenileme mantığı
    }

    return Promise.reject(error);
  }
);

export default api; 