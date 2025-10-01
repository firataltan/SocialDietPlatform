import React from 'react';
import { Box, Container, Typography, Link } from '@mui/material';

const Footer: React.FC = () => {
  return (
    <Box
      component="footer"
      sx={{
        py: 3,
        px: 2,
        mt: 'auto',
        backgroundColor: (theme) => theme.palette.grey[200],
      }}
    >
      <Container maxWidth="sm">
        <Typography variant="body1" align="center">
          © {new Date().getFullYear()} Social Diet Platform
        </Typography>
        <Typography variant="body2" color="text.secondary" align="center">
          <Link color="inherit" href="/about">
            Hakkımızda
          </Link>
          {' | '}
          <Link color="inherit" href="/contact">
            İletişim
          </Link>
          {' | '}
          <Link color="inherit" href="/privacy">
            Gizlilik Politikası
          </Link>
        </Typography>
      </Container>
    </Box>
  );
};

export default Footer; 