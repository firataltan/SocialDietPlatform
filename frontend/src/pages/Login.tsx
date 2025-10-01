import React, { useState } from 'react';
import { useNavigate, useLocation, Link as RouterLink } from 'react-router-dom';
import { useFormik } from 'formik';
import * as yup from 'yup';
import {
  Container,
  Box,
  Typography,
  TextField,
  Button,
  Link,
  Alert,
  CircularProgress,
  Paper,
  InputAdornment,
  IconButton,
  useTheme,
  useMediaQuery,
} from '@mui/material';
import { Visibility, VisibilityOff, Email, Lock } from '@mui/icons-material';
import { useAuth } from '../contexts/AuthContext';
import type { LoginCredentials } from '../types';

const validationSchema = yup.object({
  emailOrUsername: yup
    .string()
    .required('E-posta veya kullanıcı adı gereklidir'),
  password: yup
    .string()
    .min(6, 'Şifre en az 6 karakter olmalıdır')
    .required('Şifre gereklidir'),
});

const Login: React.FC = () => {
  const { login, loading, authError, authMessage, clearAuthMessages } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
  const [showPassword, setShowPassword] = useState(false);

  const from = location.state?.from?.pathname || '/';

  const formik = useFormik({
    initialValues: {
      emailOrUsername: '',
      password: '',
    } as LoginCredentials,
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      try {
        await login(values);
        navigate(from, { replace: true });
      } catch (error) {
        // Hata mesajı AuthContext tarafından set edildi, burada ekstra bir şey yapmaya gerek yok
      }
    },
  });

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  return (
    <Container
      component="main"
      maxWidth="sm"
      sx={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        py: 4,
      }}
    >
      <Paper
        elevation={3}
        sx={{
          p: { xs: 3, sm: 6 },
          width: '100%',
          borderRadius: 2,
          bgcolor: 'background.paper',
        }}
      >
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}
        >
          <Typography
            component="h1"
            variant={isMobile ? 'h4' : 'h3'}
            sx={{
              fontWeight: 'bold',
              mb: 3,
              color: 'primary.main',
            }}
          >
            Hoş Geldiniz
          </Typography>

          <Typography
            variant="body1"
            color="text.secondary"
            align="center"
            sx={{ mb: 4 }}
          >
            Sağlıklı yaşam yolculuğunuza devam etmek için giriş yapın
          </Typography>

          {authMessage && (
            <Alert severity="success" sx={{ width: '100%', mb: 3 }}>
              {authMessage}
            </Alert>
          )}

          {authError && (
            <Alert severity="error" sx={{ width: '100%', mb: 3 }}>
              {authError}
            </Alert>
          )}

          <Box component="form" onSubmit={formik.handleSubmit} sx={{ width: '100%' }}>
            <TextField
              margin="normal"
              fullWidth
              id="emailOrUsername"
              name="emailOrUsername"
              label="E-posta veya Kullanıcı Adı"
              value={formik.values.emailOrUsername}
              onChange={formik.handleChange}
              error={formik.touched.emailOrUsername && Boolean(formik.errors.emailOrUsername)}
              helperText={formik.touched.emailOrUsername && formik.errors.emailOrUsername}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <Email color="action" />
                  </InputAdornment>
                ),
              }}
            />
            <TextField
              margin="normal"
              fullWidth
              id="password"
              name="password"
              label="Şifre"
              type={showPassword ? 'text' : 'password'}
              value={formik.values.password}
              onChange={formik.handleChange}
              error={formik.touched.password && Boolean(formik.errors.password)}
              helperText={formik.touched.password && formik.errors.password}
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <Lock color="action" />
                  </InputAdornment>
                ),
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle password visibility"
                      onClick={handleClickShowPassword}
                      edge="end"
                    >
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              size="large"
              sx={{
                mt: 4,
                mb: 2,
                py: 1.5,
                borderRadius: 2,
                textTransform: 'none',
                fontSize: '1.1rem',
              }}
              disabled={loading}
            >
              {loading ? <CircularProgress size={24} /> : 'Giriş Yap'}
            </Button>
            <Box sx={{ textAlign: 'center', mt: 2 }}>
              <Link
                component={RouterLink}
                to="/register"
                variant="body1"
                sx={{
                  textDecoration: 'none',
                  '&:hover': {
                    textDecoration: 'underline',
                  },
                }}
              >
                Hesabınız yok mu? Kayıt olun
              </Link>
            </Box>
          </Box>
        </Box>
      </Paper>
    </Container>
  );
};

export default Login; 