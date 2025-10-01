import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Button,
  Box,
  Card,
  CardContent,
  CardActions,
  IconButton,
  CardMedia,
} from '@mui/material';
import { Add as AddIcon, Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { recipeService } from '../services/recipeService';
import type { Recipe } from '../types';

const Recipes: React.FC = () => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    loadRecipes();
  }, []);

  const loadRecipes = async () => {
    try {
      setLoading(true);
      const response = await recipeService.getRecipes();
      if (response && Array.isArray(response.data)) {
        setRecipes(response.data);
        setError(null);
      } else {
        setRecipes([]);
        setError('Beklenmedik veri formatı.');
        console.error('Unexpected data format for recipes:', response);
      }
    } catch (err) {
      setError('Yemek tarifleri yüklenirken bir hata oluştu.');
      console.error('Error loading recipes:', err);
      setRecipes([]);
    } finally {
      setLoading(false);
    }
  };

  // Optional: Add handleDelete if needed later
  // const handleDelete = async (id: string) => { ... };

  if (loading) {
    return (
      <Container>
        <Box sx={{ mt: 4 }}>
          <Typography>Yükleniyor...</Typography>
        </Box>
      </Container>
    );
  }

  if (error) {
    return (
      <Container>
        <Box sx={{ mt: 4 }}>
          <Typography color="error">{error}</Typography>
        </Box>
      </Container>
    );
  }

  return (
    <Container>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 4, mt: 4 }}>
        <Typography variant="h4" component="h1">
          Yemek Tarifleri
        </Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={() => navigate('/recipes/new')}
        >
          Yeni Tarif Ekle
        </Button>
      </Box>

      {recipes && recipes.length > 0 ? (
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: { xs: '1fr', sm: 'repeat(auto-fit, minmax(250px, 1fr))' },
            gap: 3,
          }}
        >
          {recipes.map((recipe) => (
            <Card key={recipe.id} sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
               {recipe.imageUrl && (
                <CardMedia
                  component="img"
                  height="140"
                  image={recipe.imageUrl}
                  alt={recipe.title}
                />
               )}
              <CardContent sx={{ flexGrow: 1 }}>
                <Typography variant="h6" component="h2" gutterBottom>
                  {recipe.title}
                </Typography>
                <Typography color="textSecondary">
                  {recipe.description}
                </Typography>
                {/* Add more recipe details here if available */}
              </CardContent>
              <CardActions>
                {/* Add buttons for View/Edit/Delete if needed */}
                 <Button size="small" onClick={() => navigate(`/recipes/${recipe.id}`)}>
                   Görüntüle
                 </Button>
                 {/* Example:
                  <IconButton size="small" onClick={() => navigate(`/recipes/${recipe.id}/edit`)}>
                    <EditIcon />
                  </IconButton>
                  <IconButton size="small" color="error" onClick={() => handleDelete(recipe.id)}>
                    <DeleteIcon />
                  </IconButton> */}
              </CardActions>
            </Card>
          ))}
        </Box>
      ) : (
        <Typography variant="h6" color="text.secondary" align="center">
          Henüz yemek tarifi eklenmemiş. İlk tarifi siz ekleyin!
        </Typography>
      )}
    </Container>
  );
};

export default Recipes;