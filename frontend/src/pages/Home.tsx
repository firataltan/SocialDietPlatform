import React from 'react';
import {
  Typography,
  Card,
  CardContent,
  CardMedia,
  Button,
  Box,
  Container,
  useTheme,
  useMediaQuery,
} from '@mui/material';
import { Link } from 'react-router-dom';
import { Restaurant, FitnessCenter, People } from '@mui/icons-material';

const Home: React.FC = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  return (
    <Box>
      {/* Hero Section */}
      <Box
        sx={{
          bgcolor: 'primary.main',
          color: 'white',
          py: 8,
          mb: 6,
          position: 'relative',
          overflow: 'hidden',
        }}
      >
        <Container maxWidth="lg">
          <Box
            sx={{
              display: 'grid',
              gridTemplateColumns: { xs: '1fr', md: '1fr 1fr' },
              gap: 4,
              alignItems: 'center',
            }}
          >
            <Box>
              <Typography
                variant={isMobile ? 'h4' : 'h2'}
                component="h1"
                gutterBottom
                sx={{ fontWeight: 'bold' }}
              >
                Sağlıklı Yaşam İçin Doğru Adres
              </Typography>
              <Typography variant="h6" paragraph sx={{ mb: 4 }}>
                Kişiselleştirilmiş diyet planları, sağlıklı tarifler ve destekleyici bir topluluk ile
                hedeflerinize ulaşın.
              </Typography>
              <Button
                component={Link}
                to="/register"
                variant="contained"
                size="large"
                sx={{
                  bgcolor: 'white',
                  color: 'primary.main',
                  '&:hover': {
                    bgcolor: 'grey.100',
                  },
                }}
              >
                Hemen Başla
              </Button>
            </Box>
            <Box>
              <Box
                component="img"
                src="https://source.unsplash.com/random/800x600/?healthy-food"
                alt="Sağlıklı Yaşam"
                sx={{
                  width: '100%',
                  height: 'auto',
                  borderRadius: 2,
                  boxShadow: 3,
                }}
              />
            </Box>
          </Box>
        </Container>
      </Box>

      {/* Features Section */}
      <Container maxWidth="lg">
        <Typography
          variant="h4"
          component="h2"
          gutterBottom
          align="center"
          sx={{ mb: 6 }}
        >
          Neler Sunuyoruz?
        </Typography>

        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: { xs: '1fr', md: 'repeat(3, 1fr)' },
            gap: 4,
          }}
        >
          <Card
            sx={{
              height: '100%',
              display: 'flex',
              flexDirection: 'column',
              transition: 'transform 0.2s',
              '&:hover': {
                transform: 'translateY(-8px)',
              },
            }}
          >
            <CardMedia
              component="img"
              height="200"
              image="https://source.unsplash.com/random/800x600/?diet"
              alt="Diyet Planları"
            />
            <CardContent sx={{ flexGrow: 1 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <FitnessCenter sx={{ mr: 1, color: 'primary.main' }} />
                <Typography gutterBottom variant="h5" component="div">
                  Diyet Planları
                </Typography>
              </Box>
              <Typography variant="body1" color="text.secondary" paragraph>
                Size özel hazırlanmış diyet planları ile hedeflerinize ulaşın. Uzmanlarımız
                tarafından kişiselleştirilmiş beslenme programları.
              </Typography>
              <Button
                component={Link}
                to="/diet-plans"
                variant="contained"
                fullWidth
              >
                Planları İncele
              </Button>
            </CardContent>
          </Card>

          <Card
            sx={{
              height: '100%',
              display: 'flex',
              flexDirection: 'column',
              transition: 'transform 0.2s',
              '&:hover': {
                transform: 'translateY(-8px)',
              },
            }}
          >
            <CardMedia
              component="img"
              height="200"
              image="https://source.unsplash.com/random/800x600/?food"
              alt="Yemek Tarifleri"
            />
            <CardContent sx={{ flexGrow: 1 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <Restaurant sx={{ mr: 1, color: 'primary.main' }} />
                <Typography gutterBottom variant="h5" component="div">
                  Yemek Tarifleri
                </Typography>
              </Box>
              <Typography variant="body1" color="text.secondary" paragraph>
                Sağlıklı ve lezzetli yemek tarifleri ile beslenmenizi çeşitlendirin. Her damak
                tadına uygun seçenekler.
              </Typography>
              <Button
                component={Link}
                to="/recipes"
                variant="contained"
                fullWidth
              >
                Tarifleri Keşfet
              </Button>
            </CardContent>
          </Card>

          <Card
            sx={{
              height: '100%',
              display: 'flex',
              flexDirection: 'column',
              transition: 'transform 0.2s',
              '&:hover': {
                transform: 'translateY(-8px)',
              },
            }}
          >
            <CardMedia
              component="img"
              height="200"
              image="https://source.unsplash.com/random/800x600/?community"
              alt="Topluluk"
            />
            <CardContent sx={{ flexGrow: 1 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                <People sx={{ mr: 1, color: 'primary.main' }} />
                <Typography gutterBottom variant="h5" component="div">
                  Topluluk
                </Typography>
              </Box>
              <Typography variant="body1" color="text.secondary" paragraph>
                Benzer hedeflere sahip kişilerle deneyimlerinizi paylaşın. Motivasyon ve destek
                için topluluğumuza katılın.
              </Typography>
              <Button
                component={Link}
                to="/community"
                variant="contained"
                fullWidth
              >
                Topluluğa Katıl
              </Button>
            </CardContent>
          </Card>
        </Box>
      </Container>
    </Box>
  );
};

export default Home; 