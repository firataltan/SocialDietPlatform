import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import * as yup from 'yup';
import {
  Container,
  Typography,
  Box,
  TextField,
  Button,
  CircularProgress,
  Alert,
  Paper,
  InputAdornment,
  useTheme,
  useMediaQuery,
  IconButton,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
} from '@mui/material';
import {
  Assignment,
  Description,
  AccessTime,
  Restaurant,
  Add as AddIcon,
  Delete as DeleteIcon,
  Image as ImageIcon
} from '@mui/icons-material';
import { recipeService } from '../services/recipeService';
import type {
  Recipe,
  RecipeIngredient,
  Food,
  RecipeFormValues,
  RecipeFormErrors,
  RecipeFormTouched,
  RecipeFormField,
  RecipeIngredientField,
  Category
} from '../types';

const validationSchema = yup.object({
  name: yup.string().required('Başlık gereklidir'),
  description: yup.string().required('Açıklama gereklidir'),
  preparationTime: yup.number().required('Hazırlık süresi gereklidir').min(0, 'Hazırlık süresi pozitif olmalıdır'),
  cookingTime: yup.number().required('Pişirme süresi gereklidir').min(0, 'Pişirme süresi pozitif olmalıdır'),
  servings: yup.number().required('Porsiyon sayısı gereklidir').min(1, 'Porsiyon sayısı en az 1 olmalıdır'),
  imageUrl: yup.string().url('Geçerli bir URL giriniz').nullable().transform((curr, orig) => orig === '' ? null : curr),
  categoryId: yup.string().required('Kategori seçimi gereklidir'),
  ingredients: yup.array().of(
    yup.object().shape({
      foodId: yup.string().required('Yemek seçimi gereklidir'),
      quantity: yup.number().required('Miktar gereklidir').min(0, 'Miktar pozitif olmalıdır'),
      unit: yup.string().required('Birim gereklidir'),
    })
  ).min(1, 'En az bir malzeme eklemelisiniz'),
  instructions: yup.string().required('Talimatlar gereklidir'),
});

const RecipeForm: React.FC = () => {
  const navigate = useNavigate();
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [categories, setCategories] = useState<Category[]>([]);
  const [foods, setFoods] = useState<Food[]>([]);
  const [dataLoading, setDataLoading] = useState(true);
  const [dataError, setDataError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setDataLoading(true);
        setDataError(null);

        const [categoriesResponse, foodsResponse] = await Promise.all([
          recipeService.getCategories(),
          recipeService.getFoods()
        ]);

        if (categoriesResponse.status === 200 && categoriesResponse.data) {
          setCategories(categoriesResponse.data);
        } else {
          setDataError(prev => (prev ? prev + ' ' : '') + 'Kategoriler getirilemedi.' + (categoriesResponse?.message || ''));
        }

        if (foodsResponse.status === 200 && foodsResponse.data) {
          setFoods(foodsResponse.data);
        } else {
          setDataError(prev => (prev ? prev + ' ' : '') + 'Yiyecekler getirilemedi.' + (foodsResponse?.message || ''));
        }

      } catch (err) {
        setDataError('Veriler getirilirken bir hata oluştu.');
        console.error('Error fetching data:', err);
      } finally {
        setDataLoading(false);
      }
    };

    fetchData();
  }, []);

  const formik = useFormik<RecipeFormValues>({
    initialValues: {
      name: '',
      description: '',
      preparationTime: 0,
      cookingTime: 0,
      servings: 1,
      imageUrl: '',
      categoryId: '',
      ingredients: [{ foodId: '', quantity: 0, unit: 'g' }],
      instructions: '',
    },
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      try {
        setLoading(true);
        setError(null);
        setSuccessMessage('');

        const recipePayload = {
          ...values,
          instructions: values.instructions.split(/\r?\n/).map(step => step.trim()).filter(step => step !== ''),
          userId: '00000000-0000-0000-0000-000000000000',
          createdAt: new Date().toISOString(),
          updatedAt: new Date().toISOString(),
        };

        const response = await recipeService.createRecipe(recipePayload as Omit<Recipe, 'id' | 'createdAt' | 'updatedAt'>);

        if (response && response.status === 200 && response.data) {
          setSuccessMessage('Tarif başarıyla oluşturuldu!');
          setTimeout(() => {
            navigate('/recipes');
          }, 1500);
        } else {
          const errorMessage = response?.message || 'Bilinmeyen bir hata oluştu.';
          setError('Tarif oluşturulurken bir hata oluştu: ' + errorMessage);
        }
      } catch (err) {
        console.error('Error creating recipe:', err);
        setError('Tarif oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.');
      } finally {
        setLoading(false);
      }
    },
  });

  const addIngredient = () => {
    formik.setFieldValue('ingredients', [
      ...formik.values.ingredients,
      { foodId: '', quantity: 0, unit: 'g' }
    ]);
  };

  const removeIngredient = (index: number) => {
    if (formik.values.ingredients.length <= 1) return;
    const newIngredients = formik.values.ingredients.filter((_, i) => i !== index);
    formik.setFieldValue('ingredients', newIngredients);
  };

  const getFieldError = (field: RecipeFormField | RecipeIngredientField, index?: number): string | undefined => {
    if (index !== undefined) {
      const ingredientErrors = formik.errors.ingredients as any;
      const ingredientTouched = formik.touched.ingredients as any;
      if (ingredientTouched?.[index] && ingredientErrors?.[index]) {
        return ingredientErrors[index][field as RecipeIngredientField];
      }
      return undefined;
    }
    const formField = field as RecipeFormField;
    return formik.touched[formField] ? (formik.errors[formField] as string | undefined) : undefined;
  };

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <Paper elevation={3} sx={{ p: { xs: 3, sm: 6 }, borderRadius: 2, bgcolor: 'background.paper' }}>
        <Box sx={{ textAlign: 'center', mb: 4 }}>
          <Typography variant={isMobile ? 'h4' : 'h3'} component="h1" gutterBottom sx={{ fontWeight: 'bold', color: 'primary.main' }}>
            Yeni Yemek Tarifi Ekle
          </Typography>
        </Box>

        {successMessage && (
          <Alert severity="success" sx={{ mb: 2 }}>
            {successMessage}
          </Alert>
        )}
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}
        {dataError && (
          <Alert severity="warning" sx={{ mb: 2 }}>
            {dataError}
          </Alert>
        )}
        {(loading || dataLoading) && (
          <Box sx={{ display: 'flex', justifyContent: 'center', mb: 2 }}>
            <CircularProgress />
          </Box>
        )}

        {!(loading || dataLoading) && !dataError && (
          <Box component="form" onSubmit={formik.handleSubmit} sx={{ width: '100%' }}>
            <TextField
              fullWidth
              id="name"
              name="name"
              label="Başlık"
              value={formik.values.name}
              onChange={formik.handleChange}
              error={Boolean(getFieldError('name'))}
              helperText={getFieldError('name')}
              margin="normal"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <Assignment color="action" />
                  </InputAdornment>
                ),
              }}
            />

            <TextField
              fullWidth
              id="description"
              name="description"
              label="Açıklama"
              multiline
              rows={3}
              value={formik.values.description}
              onChange={formik.handleChange}
              error={Boolean(getFieldError('description'))}
              helperText={getFieldError('description')}
              margin="normal"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start" sx={{ mt: 'auto' }}>
                    <Description color="action" />
                  </InputAdornment>
                ),
              }}
            />

            <TextField
              fullWidth
              id="imageUrl"
              name="imageUrl"
              label="Resim URL'si (isteğe bağlı)"
              value={formik.values.imageUrl}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={formik.touched.imageUrl && Boolean(formik.errors.imageUrl)}
              helperText={formik.touched.imageUrl && formik.errors.imageUrl}
              margin="normal"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <ImageIcon color="action" />
                  </InputAdornment>
                ),
              }}
            />

            <FormControl fullWidth margin="normal" error={Boolean(getFieldError('categoryId'))}>
              <InputLabel id="category-label">Kategori</InputLabel>
              <Select
                labelId="category-label"
                id="categoryId"
                name="categoryId"
                value={formik.values.categoryId}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                label="Kategori"
              >
                <MenuItem value=""><em>Kategori Seçin</em></MenuItem>
                {categories.map((category) => (
                  <MenuItem key={category.id} value={category.id}>
                    {category.name}
                  </MenuItem>
                ))}
              </Select>
              {Boolean(getFieldError('categoryId')) && (
                <Typography variant="caption" color="error">
                  {getFieldError('categoryId')}
                </Typography>
              )}
            </FormControl>

            <Typography variant="h6" sx={{ mt: 3, mb: 2 }}>Malzemeler</Typography>
            {formik.values.ingredients.map((ingredient, index) => (
              <Box key={index} sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(4, 1fr)' }, gap: 2, mb: 2, alignItems: 'center' }}>
                <FormControl fullWidth error={Boolean(getFieldError('foodId', index))}>
                  <InputLabel id={`food-label-${index}`}>Yemek</InputLabel>
                  <Select
                    labelId={`food-label-${index}`}
                    id={`ingredients.${index}.foodId`}
                    name={`ingredients.${index}.foodId`}
                    value={ingredient.foodId}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    label="Yemek"
                  >
                    <MenuItem value=""><em>Yemek Seçin</em></MenuItem>
                    {foods.map((food) => (
                      <MenuItem key={food.id} value={food.id}>
                        {food.name}
                      </MenuItem>
                    ))}
                  </Select>
                  {Boolean(getFieldError('foodId', index)) && (
                    <Typography variant="caption" color="error">
                      {getFieldError('foodId', index)}
                    </Typography>
                  )}
                </FormControl>

                <TextField
                  fullWidth
                  id={`ingredients.${index}.quantity`}
                  name={`ingredients.${index}.quantity`}
                  label="Miktar"
                  type="number"
                  value={ingredient.quantity}
                  onChange={formik.handleChange}
                  onBlur={formik.handleBlur}
                  error={Boolean(getFieldError('quantity', index))}
                  helperText={getFieldError('quantity', index)}
                  inputProps={{ min: 0 }}
                />

                <FormControl fullWidth error={Boolean(getFieldError('unit', index))}>
                  <InputLabel id={`unit-label-${index}`}>Birim</InputLabel>
                  <Select
                    labelId={`unit-label-${index}`}
                    id={`ingredients.${index}.unit`}
                    name={`ingredients.${index}.unit`}
                    value={ingredient.unit}
                    onChange={formik.handleChange}
                    onBlur={formik.handleBlur}
                    label="Birim"
                  >
                    <MenuItem value="g">Gram (g)</MenuItem>
                    <MenuItem value="kg">Kilogram (kg)</MenuItem>
                    <MenuItem value="ml">Mililitre (ml)</MenuItem>
                    <MenuItem value="lt">Litre (lt)</MenuItem>
                    <MenuItem value="adet">Adet</MenuItem>
                    <MenuItem value="tatlı kaşığı">Tatlı Kaşığı</MenuItem>
                    <MenuItem value="yemek kaşığı">Yemek Kaşığı</MenuItem>
                    <MenuItem value="çay kaşığı">Çay Kaşığı</MenuItem>
                    <MenuItem value="fincan">Fincan</MenuItem>
                    <MenuItem value="su bardağı">Su Bardağı</MenuItem>
                  </Select>
                  {Boolean(getFieldError('unit', index)) && (
                    <Typography variant="caption" color="error">
                      {getFieldError('unit', index)}
                    </Typography>
                  )}
                </FormControl>

                <Box sx={{ display: 'flex', alignItems: 'center', height: '100%' }}>
                  <IconButton
                    color="error"
                    onClick={() => removeIngredient(index)}
                    disabled={formik.values.ingredients.length === 1}
                    aria-label="remove ingredient"
                  >
                    <DeleteIcon />
                  </IconButton>
                </Box>
              </Box>
            ))}

            <Button
              startIcon={<AddIcon />}
              onClick={addIngredient}
              sx={{ mb: 3 }}
              variant="outlined"
            >
              Malzeme Ekle
            </Button>

            <TextField
              fullWidth
              id="instructions"
              name="instructions"
              label="Talimatlar (her adım için yeni satır)"
              multiline
              rows={4}
              value={formik.values.instructions}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={Boolean(getFieldError('instructions'))}
              helperText={getFieldError('instructions')}
              margin="normal"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start" sx={{ mt: 'auto' }}>
                    <Description color="action" />
                  </InputAdornment>
                ),
              }}
            />

            <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(3, 1fr)' }, gap: 2 }}>
              <TextField
                margin="normal"
                fullWidth
                id="preparationTime"
                name="preparationTime"
                label="Hazırlık Süresi (dk)"
                type="number"
                value={formik.values.preparationTime}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                error={Boolean(getFieldError('preparationTime'))}
                helperText={getFieldError('preparationTime')}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <AccessTime color="action" />
                    </InputAdornment>
                  ),
                }}
                inputProps={{ min: 0 }}
              />
              <TextField
                margin="normal"
                fullWidth
                id="cookingTime"
                name="cookingTime"
                label="Pişirme Süresi (dk)"
                type="number"
                value={formik.values.cookingTime}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                error={Boolean(getFieldError('cookingTime'))}
                helperText={getFieldError('cookingTime')}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <AccessTime color="action" />
                    </InputAdornment>
                  ),
                }}
                inputProps={{ min: 0 }}
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
                onBlur={formik.handleBlur}
                error={Boolean(getFieldError('servings'))}
                helperText={getFieldError('servings')}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Restaurant color="action" />
                    </InputAdornment>
                  ),
                }}
                inputProps={{ min: 1 }}
              />
            </Box>

            <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'center' }}>
              <Button
                variant="contained"
                color="primary"
                type="submit"
                disabled={formik.isSubmitting || loading || dataLoading}
                size="large"
              >
                {(formik.isSubmitting || loading) ? <CircularProgress size={24} color="inherit" /> : 'Tarif Oluştur'}
              </Button>
              <Button
                variant="outlined"
                onClick={() => navigate('/recipes')}
                disabled={formik.isSubmitting || loading || dataLoading}
                size="large"
              >
                İptal
              </Button>
            </Box>
          </Box>
        )}
      </Paper>
    </Container>
  );
};

export default RecipeForm; 