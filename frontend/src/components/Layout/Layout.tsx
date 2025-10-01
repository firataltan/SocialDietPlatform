import React from 'react';
import { Box, Container, AppBar, Toolbar, Typography, Button } from '@mui/material';
import { Outlet, Link as RouterLink } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { Alert } from '@mui/material';

const Layout: React.FC = () => {
  const { user, logout, authMessage, authError, clearAuthMessages } = useAuth();

  // Mesajları belirli bir süre sonra temizlemek için useEffect kullanabiliriz
  React.useEffect(() => {
    if (authMessage || authError) {
      const timer = setTimeout(() => {
        clearAuthMessages();
      }, 5000); // 5 saniye sonra mesajları temizle
      return () => clearTimeout(timer);
    }
  }, [authMessage, authError, clearAuthMessages]);

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <AppBar position="static">
        <Toolbar>
          {user && user.profilePictureUrl && (
            <Box sx={{ mr: 2, display: 'flex', alignItems: 'center' }}>
              <img
                src={`https://localhost:7061${user.profilePictureUrl}`} // Backend URL'i ile birleştirerek tam URL'i oluşturuyoruz
                alt="Profil Resmi"
                style={{ height: 40, width: 40, borderRadius: '50%', objectFit: 'cover' }}
              />
            </Box>
          )}
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            <Button color="inherit" component={RouterLink} to="/">
              Social Diet Platform
            </Button>
          </Typography>
          {user ? (
            <>
              <Button color="inherit" component={RouterLink} to="/profile">
                Profil
              </Button>
              <Button color="inherit" onClick={logout}>
                Çıkış Yap
              </Button>
            </>
          ) : (
            <>
              <Button color="inherit" component={RouterLink} to="/login">
                Giriş Yap
              </Button>
              <Button color="inherit" component={RouterLink} to="/register">
                Kayıt Ol
              </Button>
            </>
          )}
        </Toolbar>
      </AppBar>
      <Container component="main" sx={{ mt: 4, mb: 4, flex: 1 }}>
         {authMessage && <Alert severity="success" sx={{ mb: 2 }}>{authMessage}</Alert>}
         {authError && <Alert severity="error" sx={{ mb: 2 }}>{authError}</Alert>}
        <Outlet />
      </Container>
      {/* Footer eklenebilir */}
    </Box>
  );
};

export default Layout; 