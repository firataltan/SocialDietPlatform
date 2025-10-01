import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';

const Header: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" component={Link} to="/" sx={{ flexGrow: 1, textDecoration: 'none', color: 'inherit' }}>
          Social Diet Platform
        </Typography>
        <Box>
          <Button color="inherit" component={Link} to="/">
            Ana Sayfa
          </Button>
          {user ? (
            <>
              <Button color="inherit" component={Link} to="/diet-plans">
                Diyet Planları
              </Button>
              <Button color="inherit" component={Link} to="/recipes">
                Tarifler
              </Button>
              <Button color="inherit" component={Link} to="/profile">
                Profil
              </Button>
              <Button color="inherit" onClick={handleLogout}>
                Çıkış Yap
              </Button>
            </>
          ) : (
            <>
              <Button color="inherit" component={Link} to="/login">
                Giriş Yap
              </Button>
              <Button color="inherit" component={Link} to="/register">
                Kayıt Ol
              </Button>
            </>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header; 