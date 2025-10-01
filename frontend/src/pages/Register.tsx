import React, { useState } from 'react';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
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
  MenuItem,
  Select,
  InputLabel,
  FormControl,
  Paper,
  InputAdornment,
  IconButton,
  useTheme,
  useMediaQuery,
} from '@mui/material';
import {
  Visibility,
  VisibilityOff,
  Person,
  Email,
  Lock,
  CalendarToday,
  Badge,
} from '@mui/icons-material';
import { useAuth } from '../contexts/AuthContext';

const validationSchema = yup.object({
  firstName: yup
    .string()
    .required('Ad gereklidir')
    .min(2, 'Ad en az 2 karakter olmalıdır'),
  lastName: yup
    .string()
    .required('Soyad gereklidir')
    .min(2, 'Soyad en az 2 karakter olmalıdır'),
  userName: yup
    .string()
    .required('Kullanıcı adı gereklidir')
    .min(3, 'Kullanıcı adı en az 3 karakter olmalıdır'),
  email: yup
    .string()
    .email('Geçerli bir e-posta adresi giriniz')
    .required('E-posta adresi gereklidir'),
  password: yup
    .string()
    .min(6, 'Şifre en az 6 karakter olmalıdır')
    .required('Şifre gereklidir'),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref('password')], 'Şifreler eşleşmiyor')
    .required('Şifre tekrarı gereklidir'),
  dateOfBirth: yup
    .date()
    .required('Doğum tarihi gereklidir'),
  role: yup
    .number()
    .required('Rol gereklidir'),
});

const Register: React.FC = () => {
  const navigate = useNavigate();
  const { register, loading, authError, authMessage, clearAuthMessages } = useAuth();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const formik = useFormik({
    initialValues: {
      firstName: '',
      lastName: '',
      userName: '',
      email: '',
      password: '',
      confirmPassword: '',
      dateOfBirth: '',
      role: 0,
    },
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      try {
        await register(
          values.firstName,
          values.lastName,
          values.userName,
          values.email,
          values.password,
          values.confirmPassword,
          values.dateOfBirth,
          values.role
        );
        navigate('/login', { 
          state: { message: 'Kayıt başarılı! Lütfen giriş yapın.' }
        });
      } catch (err) {
        // Hata mesajı AuthContext tarafından set edildi, burada ekstra bir şey yapmaya gerek yok
      }
    },
  });

  const handleClickShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const handleClickShowConfirmPassword = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };

  return (
    <Container
      component="main"
      maxWidth="md"
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
              mb: 2,
              color: 'primary.main',
            }}
          >
          Kayıt Ol
        </Typography>

          <Typography
            variant="body1"
            color="text.secondary"
            align="center"
            sx={{ mb: 4 }}
          >
            Sağlıklı yaşam yolculuğunuza başlamak için hesap oluşturun
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

          <Box 
            component="form" 
            onSubmit={formik.handleSubmit} 
            sx={{ 
              width: '100%',
              display: 'grid',
              gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)' },
              gap: 2,
            }}
          >
            <Box>
          <TextField
            fullWidth
            id="firstName"
            name="firstName"
            label="Ad"
            value={formik.values.firstName}
            onChange={formik.handleChange}
            error={formik.touched.firstName && Boolean(formik.errors.firstName)}
            helperText={formik.touched.firstName && formik.errors.firstName}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Person color="action" />
                    </InputAdornment>
                  ),
                }}
              />
            </Box>
            <Box>
          <TextField
            fullWidth
            id="lastName"
            name="lastName"
            label="Soyad"
            value={formik.values.lastName}
            onChange={formik.handleChange}
            error={formik.touched.lastName && Boolean(formik.errors.lastName)}
            helperText={formik.touched.lastName && formik.errors.lastName}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Person color="action" />
                    </InputAdornment>
                  ),
                }}
              />
            </Box>
            <Box sx={{ gridColumn: { xs: '1', sm: '1 / -1' } }}>
          <TextField
            fullWidth
            id="userName"
            name="userName"
            label="Kullanıcı Adı"
            value={formik.values.userName}
            onChange={formik.handleChange}
            error={formik.touched.userName && Boolean(formik.errors.userName)}
            helperText={formik.touched.userName && formik.errors.userName}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Badge color="action" />
                    </InputAdornment>
                  ),
                }}
              />
            </Box>
            <Box sx={{ gridColumn: { xs: '1', sm: '1 / -1' } }}>
          <TextField
            fullWidth
            id="email"
            name="email"
            label="E-posta Adresi"
            value={formik.values.email}
            onChange={formik.handleChange}
            error={formik.touched.email && Boolean(formik.errors.email)}
            helperText={formik.touched.email && formik.errors.email}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Email color="action" />
                    </InputAdornment>
                  ),
                }}
              />
            </Box>
            <Box>
          <TextField
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
            </Box>
            <Box>
          <TextField
            fullWidth
            id="confirmPassword"
            name="confirmPassword"
            label="Şifre Tekrarı"
                type={showConfirmPassword ? 'text' : 'password'}
            value={formik.values.confirmPassword}
            onChange={formik.handleChange}
            error={formik.touched.confirmPassword && Boolean(formik.errors.confirmPassword)}
            helperText={formik.touched.confirmPassword && formik.errors.confirmPassword}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Lock color="action" />
                    </InputAdornment>
                  ),
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        aria-label="toggle confirm password visibility"
                        onClick={handleClickShowConfirmPassword}
                        edge="end"
                      >
                        {showConfirmPassword ? <VisibilityOff /> : <Visibility />}
                      </IconButton>
                    </InputAdornment>
                  ),
                }}
              />
            </Box>
            <Box>
          <TextField
            fullWidth
            id="dateOfBirth"
            name="dateOfBirth"
            label="Doğum Tarihi"
            type="date"
            InputLabelProps={{ shrink: true }}
            value={formik.values.dateOfBirth}
            onChange={formik.handleChange}
            error={formik.touched.dateOfBirth && Boolean(formik.errors.dateOfBirth)}
            helperText={formik.touched.dateOfBirth && formik.errors.dateOfBirth}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <CalendarToday color="action" />
                    </InputAdornment>
                  ),
                }}
              />
            </Box>
            <Box>
              <FormControl fullWidth required>
            <InputLabel id="role-label">Rol</InputLabel>
            <Select
              labelId="role-label"
              id="role"
              name="role"
              value={formik.values.role}
              label="Rol"
              onChange={formik.handleChange}
              error={formik.touched.role && Boolean(formik.errors.role)}
            >
              <MenuItem value={0}>Kullanıcı</MenuItem>
              <MenuItem value={1}>Diyetisyen</MenuItem>
            </Select>
            {formik.touched.role && formik.errors.role && (
              <Typography variant="caption" color="error">{formik.errors.role}</Typography>
            )}
          </FormControl>
            </Box>

            <Box sx={{ gridColumn: { xs: '1', sm: '1 / -1' } }}>
          <Button
            type="submit"
            fullWidth
            variant="contained"
                size="large"
                sx={{
                  mt: 2,
                  py: 1.5,
                  borderRadius: 2,
                  textTransform: 'none',
                  fontSize: '1.1rem',
                }}
            disabled={loading}
          >
            {loading ? <CircularProgress size={24} /> : 'Kayıt Ol'}
          </Button>
            </Box>

            <Box sx={{ gridColumn: { xs: '1', sm: '1 / -1' }, textAlign: 'center', mt: 2 }}>
              <Link
                component={RouterLink}
                to="/login"
                variant="body1"
                sx={{
                  textDecoration: 'none',
                  '&:hover': {
                    textDecoration: 'underline',
                  },
                }}
              >
              Zaten hesabınız var mı? Giriş yapın
            </Link>
          </Box>
        </Box>
      </Box>
      </Paper>
    </Container>
  );
};

export default Register; 