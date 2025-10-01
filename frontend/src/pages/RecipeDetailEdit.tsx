import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import * as yup from 'yup';
import {
  Container,
  Typography,
  Box,
  CircularProgress,
  Alert,
  Button,
  TextField,
  Paper,
  InputAdornment,
  IconButton,
  List,
  ListItem,
  ListItemText,
} from '@mui/material';
import {
  Edit as EditIcon,
  Save as SaveIcon,
  Cancel as CancelIcon,
  Person,
  Description,
  AccessTime,
  Restaurant,
  FitnessCenter,
  Assignment,
} from '@mui/icons-material';
// Assuming recipeService and Recipe type exist and ApiResponse
import { recipeService } from '../services/recipeService';
import type { Recipe, ApiResponse } from '../types';

const validationSchema = yup.object({
  title: yup.string().required('Başlık gereklidir'),
  description: yup.string().required('Açıklama gereklidir'),
  prepTime: yup.number().required('Hazırlık süresi gereklidir').min(0, 'Hazırlık süresi pozitif olmalıdır'),
  cookTime: yup.number().required('Pişirme süresi gereklidir').min(0, 'Pişirme süresi pozitif olmalıdır'),
  servings: yup.number().required('Porsiyon sayısı gereklidir').min(1, 'Porsiyon sayısı en az 1 olmalıdır'),
  calories: yup.number().required('Kalori gereklidir').min(0, 'Kalori pozitif olmalıdır'),
  // ingredients ve instructions validation daha karmaşık olabilir, şimdilik temel alanlar
});

const RecipeDetailEdit: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [recipe, setRecipe] = useState<Recipe | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [successMessage, setSuccessMessage] = useState<string>('');

  useEffect(() => {
    if (id) {
      loadRecipe(id);
    }
  }, [id]);

  const loadRecipe = async (recipeId: string) => {
    try {
      setLoading(true);
      setError(null);
      // Assuming getRecipe returns a response object with a single recipe data
      const response: ApiResponse<Recipe> = await recipeService.getRecipe(recipeId);
      if (response && response.data) {
        setRecipe(response.data);
      } else {
        setRecipe(null);
        setError('Tarif bulunamadı.');
      }
    } catch (err) {
      setError('Tarif yüklenirken bir hata oluştu.');
      console.error('Error loading recipe:', err);
    } finally {
      setLoading(false);
    }
  };

  const formik = useFormik({
    initialValues: {
      id: recipe?.id || '',
      title: recipe?.title || '',
      description: recipe?.description || '',
      ingredients: recipe?.ingredients || [],
      instructions: recipe?.instructions || [],
      prepTime: recipe?.prepTime || 0,
      cookTime: recipe?.cookTime || 0,
      servings: recipe?.servings || 1,
      calories: recipe?.calories || 0,
      protein: recipe?.protein || 0,
      carbs: recipe?.carbs || 0,
      fat: recipe?.fat || 0,
      imageUrl: recipe?.imageUrl || '',
      userId: recipe?.userId || '',
      createdAt: recipe?.createdAt || '',
      updatedAt: recipe?.updatedAt || '',
    } as Recipe, // Type assertion based on expected structure after fetching
    enableReinitialize: true,
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      try {
        setLoading(true);
        setError(null);
        setSuccessMessage('');
        // Assuming updateRecipe exists and returns a response
        const response = await recipeService.updateRecipe(id!, values);
        if (response && response.data) {
          setRecipe(response.data);
          setSuccessMessage('Tarif başarıyla güncellendi!');
          setIsEditing(false);
        } else {
           setError('Tarif güncellenirken bir hata oluştu.');
        }
      } catch (err) {
        setError('Tarif güncellenirken bir hata oluştu.');
        console.error('Error updating recipe:', err);
      } finally {
        setLoading(false);
      }
    },
  });

  if (loading) {
    return (
      <Container>
        <Box sx={{ mt: 4, textAlign: 'center' }}>
          <CircularProgress />
        </Box>
      </Container>
    );
  }

  if (error) {
    return (
      <Container>
         <Box sx={{ mt: 4 }}>
           <Alert severity="error">{error}</Alert>
         </Box>
      </Container>
    );
  }

  if (!recipe) {
     return (
      <Container>
         <Box sx={{ mt: 4 }}>
           <Typography variant="h6">Tarif bulunamadı.</Typography>
         </Box>
      </Container>
    );
  }

  return (
    <Container maxWidth="md" sx={{ py: 4 }}>
      <Paper elevation={3} sx={{ p: { xs: 3, sm: 6 }, borderRadius: 2, bgcolor: 'background.paper' }}>
        <Box sx={{ textAlign: 'center', mb: 4 }}>
          <Typography variant="h4" component="h1" gutterBottom sx={{ fontWeight: 'bold', color: 'primary.main' }}>
            {isEditing ? 'Tarifi Düzenle' : recipe.title}
          </Typography>
        </Box>

         {successMessage && <Alert severity="success" sx={{ mb: 2 }}>{successMessage}</Alert>}

        {!isEditing ? (
          <Box>
            {recipe.imageUrl && (
              <Box sx={{ mb: 3, textAlign: 'center' }}>
                <Box
                  component="img"
                  src={recipe.imageUrl}
                  alt={recipe.title}
                  sx={{
                    maxWidth: '100%',
                    height: 'auto',
                    borderRadius: 2,
                    boxShadow: 3,
                  }}
                />
              </Box>
            )}
            <Typography variant="h6" gutterBottom>Açıklama:</Typography>
            <Typography variant="body1" paragraph>{recipe.description}</Typography>

            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 3, mb: 3 }}>
              <Box>
                <Typography variant="h6">Hazırlık Süresi:</Typography>
                <Typography variant="body1">{recipe.prepTime} dk</Typography>
              </Box>
              <Box>
                <Typography variant="h6">Pişirme Süresi:</Typography>
                <Typography variant="body1">{recipe.cookTime} dk</Typography>
              </Box>
              <Box>
                <Typography variant="h6">Porsiyon:</Typography>
                <Typography variant="body1">{recipe.servings}</Typography>
              </Box>
              <Box>
                <Typography variant="h6">Kalori:</Typography>
                <Typography variant="body1">{recipe.calories} kcal</Typography>
              </Box>
            </Box>

            <Typography variant="h6" gutterBottom>Malzemeler:</Typography>
            <List>
              {recipe.ingredients.map((item, index) => (
                <ListItem key={index} disablePadding>
                  <ListItemText primary={`• ${item}`} />
                </ListItem>
              ))}
            </List>

            <Typography variant="h6" gutterBottom sx={{ mt: 3 }}>Talimatlar:</Typography>
             <List>
              {recipe.instructions.map((step, index) => (
                <ListItem key={index} disablePadding>
                  <ListItemText primary={`${index + 1}. ${step}`} />
                </ListItem>
              ))}
            </List>

            <Box sx={{ mt: 4, textAlign: 'center' }}>
              <Button variant="contained" onClick={() => setIsEditing(true)} startIcon={<EditIcon />}>
                Tarifi Düzenle
              </Button>
            </Box>
          </Box>
        ) : (
          <Box component="form" onSubmit={formik.handleSubmit} sx={{ textAlign: 'left' }}>
             <TextField
                margin="normal"
                fullWidth
                id="title"
                name="title"
                label="Başlık"
                value={formik.values.title}
                onChange={formik.handleChange}
                error={formik.touched.title && Boolean(formik.errors.title)}
                helperText={formik.touched.title && formik.errors.title}
                 InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Assignment color="action" />
                    </InputAdornment>
                  ),
                }}
              />
              <TextField
                margin="normal"
                fullWidth
                id="description"
                name="description"
                label="Açıklama"
                multiline
                rows={4}
                value={formik.values.description}
                onChange={formik.handleChange}
                error={formik.touched.description && Boolean(formik.errors.description)}
                helperText={formik.touched.description && formik.errors.description}
                 InputProps={{
                  startAdornment: (
                    <InputAdornment position="start" sx={{ mt: 'auto' }}>
                      <Description color="action" />
                    </InputAdornment>
                  ),
                }}
              />

               <TextField
                margin="normal"
                fullWidth
                id="imageUrl"
                name="imageUrl"
                label="Resim URL'si"
                value={formik.values.imageUrl}
                onChange={formik.handleChange}
                error={formik.touched.imageUrl && Boolean(formik.errors.imageUrl)}
                helperText={formik.touched.imageUrl && formik.errors.imageUrl}
                 InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Restaurant color="action" />
                    </InputAdornment>
                  ),
                }}
              />

              {/* Add fields for ingredients and instructions - more complex input required */}
               {/* Example simple text fields:
               <TextField
                margin="normal"
                fullWidth
                id="ingredients"
                name="ingredients"
                label="Malzemeler (virgülle ayırın)"
                value={formik.values.ingredients.join(',')}
                onChange={(e) => formik.setFieldValue('ingredients', e.target.value.split(',').map(item => item.trim()))}
                error={formik.touched.ingredients && Boolean(formik.errors.ingredients)}
                helperText={formik.touched.ingredients && formik.errors.ingredients}
              />
                <TextField
                margin="normal"
                fullWidth
                id="instructions"
                name="instructions"
                label="Talimatlar (her adım için yeni satır)"
                multiline
                rows={4}
                 value={formik.values.instructions.join('\n')}
                onChange={(e) => formik.setFieldValue('instructions', e.target.value.split('\n').map(step => step.trim()))}
                error={formik.touched.instructions && Boolean(formik.errors.instructions)}
                helperText={formik.touched.instructions && formik.errors.instructions}
              /> */}

               <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(3, 1fr)' }, gap: 2 }}>
                <TextField
                  margin="normal"
                  fullWidth
                  id="prepTime"
                  name="prepTime"
                  label="Hazırlık Süresi (dk)"
                  type="number"
                  value={formik.values.prepTime}
                  onChange={formik.handleChange}
                  error={formik.touched.prepTime && Boolean(formik.errors.prepTime)}
                  helperText={formik.touched.prepTime && formik.errors.prepTime}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <AccessTime color="action" />
                      </InputAdornment>
                    ),
                  }}
                />
                 <TextField
                  margin="normal"
                  fullWidth
                  id="cookTime"
                  name="cookTime"
                  label="Pişirme Süresi (dk)"
                  type="number"
                  value={formik.values.cookTime}
                  onChange={formik.handleChange}
                  error={formik.touched.cookTime && Boolean(formik.errors.cookTime)}
                  helperText={formik.touched.cookTime && formik.errors.cookTime}
                   InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <AccessTime color="action" />
                      </InputAdornment>
                    ),
                  }}
                />
                 <TextField
                  margin="normal"
                  fullWidth
                  id="servings"
                  name="servings"
                  label="Porsiyon"
                  type="number"
                  value={formik.values.servings}
                  onChange={formik.handleChange}
                  error={formik.touched.servings && Boolean(formik.errors.servings)}
                  helperText={formik.touched.servings && formik.errors.servings}
                   InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <Restaurant color="action" />
                      </InputAdornment>
                    ),
                  }}
                />
              </Box>

               <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(3, 1fr)' }, gap: 2 }}>
                 <TextField
                  margin="normal"
                  fullWidth
                  id="calories"
                  name="calories"
                  label="Kalori (kcal)"
                  type="number"
                  value={formik.values.calories}
                  onChange={formik.handleChange}
                  error={formik.touched.calories && Boolean(formik.errors.calories)}
                  helperText={formik.touched.calories && formik.errors.calories}
                   InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <FitnessCenter color="action" />
                      </InputAdornment>
                    ),
                  }}
                />
                 {/* Add Protein, Carbs, Fat fields */}
              </Box>

            <Box sx={{ mt: 3, display: 'flex', gap: 2, justifyContent: 'center' }}>
              <Button variant="contained" color="primary" type="submit" disabled={formik.isSubmitting || loading} startIcon={<SaveIcon />}>
                {formik.isSubmitting || loading ? <CircularProgress size={24} /> : 'Kaydet'}
              </Button>
              <Button variant="outlined" color="secondary" onClick={() => {
                setIsEditing(false);
                 if (recipe) formik.resetForm({ values: recipe }); // Reset form to fetched recipe data
              }} startIcon={<CancelIcon />}>
                İptal
              </Button>
            </Box>
          </Box>
        )}
      </Paper>
    </Container>
  );
};

export default RecipeDetailEdit; 