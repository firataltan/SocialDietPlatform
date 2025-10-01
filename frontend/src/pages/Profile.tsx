import React, { useState, useEffect } from 'react';
import { Typography, Container, Box, CircularProgress, TextField, Button, Paper, Alert, InputAdornment, IconButton, useTheme, useMediaQuery } from '@mui/material';
import { useAuth } from '../contexts/AuthContext';
import { useFormik } from 'formik';
import * as yup from 'yup';
import type { User, UpdateProfilePayload } from '../types';
import { authService } from '../services/authService';
import {
  Person,
  Email,
  CalendarToday,
  FitnessCenter,
  Height,
  Description,
  Save,
  Edit as EditIcon,
  CameraAlt,
} from '@mui/icons-material';

const validationSchema = yup.object({
  firstName: yup.string().required('Ad gereklidir'),
  lastName: yup.string().required('Soyad gereklidir'),
  // email: yup.string().email('Geçerli bir e-posta adresi giriniz').required('E-posta gereklidir'), // E-posta düzenlenemez
  // Diğer alanlar için validation buraya eklenecek
  dateOfBirth: yup.date().nullable(),
  bio: yup.string().max(500, 'Biyografi en fazla 500 karakter olabilir.').nullable(),
  weight: yup.number().nullable().positive('Kilo pozitif bir değer olmalıdır.'),
  height: yup.number().nullable().positive('Boy pozitif bir değer olmalıdır.'),
  targetWeight: yup.number().nullable().positive('Hedef Kilo pozitif bir değer olmalıdır.'),
});

const Profile: React.FC = () => {
  const { user, loading, updateUser } = useAuth();
  const [isEditing, setIsEditing] = useState(false);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [errorMessage, setErrorMessage] = useState<string>('');
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [isUploading, setIsUploading] = useState(false);

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  const formik = useFormik({
    initialValues: {
      id: user?.id || '',
      firstName: user?.firstName || '',
      lastName: user?.lastName || '',
      userName: user?.userName || '',
      email: user?.email || '',
      profileImage: user?.profileImage || undefined,
      createdAt: user?.createdAt || '',
      updatedAt: user?.updatedAt || '',
      dateOfBirth: user?.dateOfBirth ? new Date(user.dateOfBirth).toISOString().split('T')[0] : '',
      role: user?.role || 0,
      bio: user?.bio || '',
      weight: user?.weight || undefined,
      height: user?.height || undefined,
      targetWeight: user?.targetWeight || undefined,
      profilePictureUrl: user?.profilePictureUrl || undefined,
    } as User,
    enableReinitialize: true,
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      try {
        setSuccessMessage('');
        setErrorMessage('');
        setIsUploading(true);

        let profileImageUrl = values.profilePictureUrl;

        if (selectedFile) {
          const uploadResponse = await authService.uploadProfileImage(selectedFile);
          profileImageUrl = uploadResponse.url;
        }

        const payload: UpdateProfilePayload = {
          firstName: values.firstName,
          lastName: values.lastName,
          dateOfBirth: values.dateOfBirth || undefined,
          bio: values.bio || undefined,
          weight: values.weight || undefined,
          height: values.height || undefined,
          targetWeight: values.targetWeight || undefined,
          profilePictureUrl: profileImageUrl,
        };

        const response = await authService.updateProfile(payload);
        if (response && response.data) {
          console.log('Backend from updateProfile:', response.data);
          const updatedUserData: User = {
            id: response.data.userId,
            firstName: response.data.firstName,
            lastName: response.data.lastName,
            userName: response.data.userName,
            email: response.data.email,
            profileImage: response.data.profilePictureUrl,
            createdAt: user?.createdAt || '',
            updatedAt: response.data.updatedAt,
            dateOfBirth: response.data.dateOfBirth,
            role: response.data.role,
            bio: response.data.bio,
            weight: response.data.weight,
            height: response.data.height,
            targetWeight: response.data.targetWeight,
          };
          updateUser(updatedUserData);
          console.log('AuthContext user after update:', updatedUserData);
        }
        setSuccessMessage('Profil başarıyla güncellendi!');
        setIsEditing(false);
      } catch (error: any) {
        console.error('Profil güncelleme hatası:', error);
        setErrorMessage('Profil güncellenirken bir hata oluştu.' + (error.response?.data?.message || ''));
      } finally {
        setIsUploading(false);
        setSelectedFile(null);
      }
    },
  });

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.currentTarget.files && event.currentTarget.files[0]) {
      setSelectedFile(event.currentTarget.files[0]);
    } else {
      setSelectedFile(null);
    }
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="80vh">
        <CircularProgress />
      </Box>
    );
  }

  if (!user) {
    return <Typography variant="h6">Kullanıcı bilgileri yüklenemedi.</Typography>;
  }

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Paper elevation={3} sx={{ p: { xs: 3, sm: 6 }, borderRadius: 2, bgcolor: 'background.paper' }}>
        <Box sx={{ textAlign: 'center', mb: 4 }}>
          <Typography variant={isMobile ? 'h4' : 'h3'} component="h1" gutterBottom sx={{ fontWeight: 'bold', color: 'primary.main' }}>
            Profilim
          </Typography>
        </Box>

        {successMessage && <Alert severity="success" sx={{ mb: 2 }}>{successMessage}</Alert>}
        {errorMessage && <Alert severity="error" sx={{ mb: 2 }}>{errorMessage}</Alert>}

        {!isEditing ? (
          <Box>
            <Box sx={{ display: 'flex', justifyContent: 'center', mb: 3 }}>
              <Box
                component="img"
                src={user.profilePictureUrl || 'https://via.placeholder.com/150'}
                alt="Profil Resmi"
                sx={{
                  width: 150,
                  height: 150,
                  borderRadius: '50%',
                  objectFit: 'cover',
                  border: '3px solid', // Added border
                  borderColor: 'primary.main', // Border color
                }}
              />
            </Box>

            <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)' }, gap: 3, textAlign: 'left' }}>
              <Box>
                <Typography variant="h6">Ad:</Typography>
                <Typography variant="body1">{user.firstName}</Typography>
              </Box>
              <Box>
                <Typography variant="h6">Soyad:</Typography>
                <Typography variant="body1">{user.lastName}</Typography>
              </Box>
              <Box sx={{ gridColumn: { xs: '1', sm: '1 / -1' } }}>
                <Typography variant="h6">Kullanıcı Adı:</Typography>
                <Typography variant="body1">{user.userName}</Typography>
              </Box>
              <Box sx={{ gridColumn: { xs: '1', sm: '1 / -1' } }}>
                <Typography variant="h6">E-posta:</Typography>
                <Typography variant="body1">{user.email}</Typography>
              </Box>

              {user.dateOfBirth && (
                <Box>
                  <Typography variant="h6">Doğum Tarihi:</Typography>
                  <Typography variant="body1">{new Date(user.dateOfBirth).toLocaleDateString()}</Typography>
                </Box>
              )}

              {user.role !== undefined && (
                <Box>
                  <Typography variant="h6">Rol:</Typography>
                  <Typography variant="body1">{user.role === 0 ? 'Kullanıcı' : 'Diyetisyen'}</Typography>
                </Box>
              )}

              {user.bio && (
                 <Box sx={{ gridColumn: { xs: '1', sm: '1 / -1' } }}>
                   <Typography variant="h6">Biyografi:</Typography>
                   <Typography variant="body1">{user.bio}</Typography>
                 </Box>
              )}

              {user.weight !== undefined && user.weight !== null && (
                <Box>
                  <Typography variant="h6">Kilo:</Typography>
                  <Typography variant="body1">{user.weight} kg</Typography>
                </Box>
              )}

              {user.height !== undefined && user.height !== null && (
                <Box>
                  <Typography variant="h6">Boy:</Typography>
                  <Typography variant="body1">{user.height} cm</Typography>
                </Box>
              )}

              {user.targetWeight !== undefined && user.targetWeight !== null && (
                 <Box>
                   <Typography variant="h6">Hedef Kilo:</Typography>
                   <Typography variant="body1">{user.targetWeight} kg</Typography>
                 </Box>
              )}

            </Box>

            <Box sx={{ mt: 4, textAlign: 'center' }}>
              <Button variant="contained" onClick={() => setIsEditing(true)} startIcon={<EditIcon />}>
                Profili Düzenle
              </Button>
            </Box>
          </Box>
        ) : (
          <Box component="form" onSubmit={formik.handleSubmit} sx={{ textAlign: 'left' }}>
            <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)' }, gap: 2 }}>
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
              {/* Email and Username are typically not editable in a profile form */}
              {/* <TextField
                margin="normal"
                fullWidth
                id="userName"
                name="userName"
                label="Kullanıcı Adı"
                value={formik.values.userName}
                onChange={formik.handleChange}
                error={formik.touched.userName && Boolean(formik.errors.userName)}
                helperText={formik.touched.userName && formik.errors.userName}
              />
               <TextField
                margin="normal"
                fullWidth
                id="email"
                name="email"
                label="E-posta Adresi"
                value={formik.values.email}
                onChange={formik.handleChange}
                error={formik.touched.email && Boolean(formik.errors.email)}
                helperText={formik.touched.email && formik.errors.email}
                disabled // Assuming email is not editable
              /> */}
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
                <TextField
                  fullWidth
                  id="weight"
                  name="weight"
                  label="Kilo (kg)"
                  type="number"
                  value={formik.values.weight}
                  onChange={formik.handleChange}
                  error={formik.touched.weight && Boolean(formik.errors.weight)}
                  helperText={formik.touched.weight && formik.errors.weight}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <FitnessCenter color="action" />
                      </InputAdornment>
                    ),
                  }}
                />
              </Box>
               <Box>
                <TextField
                  fullWidth
                  id="height"
                  name="height"
                  label="Boy (cm)"
                  type="number"
                  value={formik.values.height}
                  onChange={formik.handleChange}
                  error={formik.touched.height && Boolean(formik.errors.height)}
                  helperText={formik.touched.height && formik.errors.height}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <Height color="action" />
                      </InputAdornment>
                    ),
                  }}
                />
              </Box>
              <Box>
                <TextField
                  fullWidth
                  id="targetWeight"
                  name="targetWeight"
                  label="Hedef Kilo (kg)"
                  type="number"
                  value={formik.values.targetWeight}
                  onChange={formik.handleChange}
                  error={formik.touched.targetWeight && Boolean(formik.errors.targetWeight)}
                  helperText={formik.touched.targetWeight && formik.errors.targetWeight}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <FitnessCenter color="action" />
                      </InputAdornment>
                    ),
                  }}
                />
              </Box>
              <Box sx={{ gridColumn: { xs: '1', sm: '1 / -1' } }}>
                <TextField
                  fullWidth
                  id="bio"
                  name="bio"
                  label="Biyografi"
                  multiline
                  rows={4}
                  value={formik.values.bio}
                  onChange={formik.handleChange}
                  error={formik.touched.bio && Boolean(formik.errors.bio)}
                  helperText={formik.touched.bio && formik.errors.bio}
                   InputProps={{
                    startAdornment: (
                      <InputAdornment position="start" sx={{ mt: 'auto' }}>
                        <Description color="action" />
                      </InputAdornment>
                    ),
                  }}
                />
              </Box>

              <Box sx={{ gridColumn: { xs: '1', sm: '1 / -1' } }}>
                <Typography variant="subtitle1" gutterBottom>Profil Resmi Yükle</Typography>
                 <input
                   accept="image/*"
                   style={{ display: 'none' }}
                   id="profile-picture-upload"
                   type="file"
                   onChange={handleFileChange}
                 />
                 <label htmlFor="profile-picture-upload">
                   <Button variant="outlined" component="span" startIcon={<CameraAlt />}>
                     Resim Seç
                   </Button>
                 </label>
                 {selectedFile && <Typography variant="body2" sx={{ mt: 1 }}>Seçilen Dosya: {selectedFile.name}</Typography>}
                 {user.profilePictureUrl && !selectedFile && (
                   <Box sx={{ mt: 2 }}>
                     <Typography variant="subtitle2" gutterBottom>Mevcut Resim:</Typography>
                     <Box
                        component="img"
                        src={user.profilePictureUrl}
                        alt="Mevcut Profil Resmi"
                        sx={{
                          width: 80,
                          height: 80,
                          borderRadius: '50%',
                          objectFit: 'cover',
                          border: '2px solid', // Added border
                          borderColor: 'grey.300', // Border color
                        }}
                      />
                   </Box>
                 )}
              </Box>
            </Box>

            <Box sx={{ mt: 3, display: 'flex', gap: 2, justifyContent: 'center' }}>
              <Button variant="contained" color="primary" type="submit" disabled={formik.isSubmitting || isUploading} startIcon={<Save />}>
                {formik.isSubmitting || isUploading ? <CircularProgress size={24} /> : 'Kaydet'}
              </Button>
              <Button variant="outlined" color="secondary" onClick={() => {
                setIsEditing(false);
                formik.resetForm({ values: formik.initialValues }); // Reset form to initial values
                setSelectedFile(null); // Clear selected file on cancel
              }}>
                İptal
              </Button>
            </Box>
          </Box>
        )}
      </Paper>
    </Container>
  );
};

export default Profile; 