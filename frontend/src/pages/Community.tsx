import React from 'react';
import { Container, Typography, Box } from '@mui/material';

const Community: React.FC = () => {
  return (
    <Container>
      <Box sx={{ mt: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Topluluk
        </Typography>
        <Typography variant="body1">
          Burada topluluk içerikleri (forum, gruplar vb.) yer alacak.
        </Typography>
      </Box>
    </Container>
  );
};

export default Community; 