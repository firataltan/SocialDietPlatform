import { useState } from 'react';
import {
  Box,
  TextField,
  Button,
  IconButton,
  Typography,
  Paper,
  MenuItem,
  Divider,
} from '@mui/material';
import { Delete as DeleteIcon, Add as AddIcon } from '@mui/icons-material';
import { useFormik } from 'formik';
import * as yup from 'yup';
import FoodSearch from '../FoodSearch/FoodSearch';
import type { Meal, Food, MealFood } from '../../types';

const validationSchema = yup.object({
  name: yup.string().required('Öğün adı zorunludur'),
  description: yup.string(),
  mealTime: yup.string().required('Öğün saati zorunludur'),
  foods: yup.array().of(
    yup.object({
      food: yup.object().nullable().required('Yemek seçimi zorunludur'),
      quantity: yup.number().required('Miktar zorunludur').min(0, 'Miktar 0\'dan büyük olmalıdır'),
      unit: yup.string().required('Birim zorunludur'),
      calories: yup.number().required(),
    })
  ).min(1, 'En az bir yemek eklemelisiniz'),
});

const MEAL_TIMES = [
  { value: 'BREAKFAST', label: 'Kahvaltı' },
  { value: 'LUNCH', label: 'Öğle Yemeği' },
  { value: 'DINNER', label: 'Akşam Yemeği' },
  { value: 'SNACK', label: 'Ara Öğün' },
];

const UNITS = [
  { value: 'g', label: 'Gram' },
  { value: 'ml', label: 'Mililitre' },
  { value: 'adet', label: 'Adet' },
  { value: 'porsiyon', label: 'Porsiyon' },
];

interface MealFormProps {
  initialValues?: Partial<Meal>;
  onSubmit: (values: Omit<Meal, 'id' | 'createdAt' | 'updatedAt'>) => Promise<void>;
  onCancel: () => void;
}

export default function MealForm({ initialValues, onSubmit, onCancel }: MealFormProps) {
  const [loading, setLoading] = useState(false);

  const formik = useFormik({
    initialValues: {
      name: initialValues?.name || '',
      description: initialValues?.description || '',
      mealTime: initialValues?.mealTime || '',
      foods: initialValues?.foods || [],
      totalCalories: initialValues?.totalCalories || 0,
    },
    validationSchema,
    onSubmit: async (values) => {
      try {
        setLoading(true);
        const totalCalories = values.foods.reduce((sum, mealFood) => sum + mealFood.calories, 0);
        await onSubmit({ ...values, totalCalories });
      } catch (error) {
        console.error('Error submitting meal:', error);
      } finally {
        setLoading(false);
      }
    },
  });

  const handleAddFood = () => {
    formik.setFieldValue('foods', [
      ...formik.values.foods,
      { food: null, quantity: 0, unit: 'g', calories: 0 },
    ]);
  };

  const handleRemoveFood = (index: number) => {
    const newFoods = [...formik.values.foods];
    newFoods.splice(index, 1);
    formik.setFieldValue('foods', newFoods);
  };

  const handleFoodChange = (index: number, food: Food | null) => {
    const newFoods = [...formik.values.foods];
    newFoods[index] = {
      ...newFoods[index],
      food,
      calories: food ? (food.calories * newFoods[index].quantity) / food.servingSize : 0,
    };
    formik.setFieldValue('foods', newFoods);
  };

  const handleQuantityChange = (index: number, quantity: number) => {
    const newFoods = [...formik.values.foods];
    const food = newFoods[index].food;
    newFoods[index] = {
      ...newFoods[index],
      quantity,
      calories: food ? (food.calories * quantity) / food.servingSize : 0,
    };
    formik.setFieldValue('foods', newFoods);
  };

  const totalCalories = formik.values.foods.reduce((sum, mealFood) => sum + mealFood.calories, 0);

  return (
    <form onSubmit={formik.handleSubmit}>
      <Box sx={{ mb: 3 }}>
        <TextField
          fullWidth
          id="name"
          name="name"
          label="Öğün Adı"
          value={formik.values.name}
          onChange={formik.handleChange}
          error={formik.touched.name && Boolean(formik.errors.name)}
          helperText={formik.touched.name && formik.errors.name}
          margin="normal"
        />

        <TextField
          fullWidth
          id="description"
          name="description"
          label="Açıklama"
          multiline
          rows={2}
          value={formik.values.description}
          onChange={formik.handleChange}
          error={formik.touched.description && Boolean(formik.errors.description)}
          helperText={formik.touched.description && formik.errors.description}
          margin="normal"
        />

        <TextField
          fullWidth
          select
          id="mealTime"
          name="mealTime"
          label="Öğün Saati"
          value={formik.values.mealTime}
          onChange={formik.handleChange}
          error={formik.touched.mealTime && Boolean(formik.errors.mealTime)}
          helperText={formik.touched.mealTime && formik.errors.mealTime}
          margin="normal"
        >
          {MEAL_TIMES.map((option) => (
            <MenuItem key={option.value} value={option.value}>
              {option.label}
            </MenuItem>
          ))}
        </TextField>
      </Box>

      <Box sx={{ mb: 3 }}>
        <Typography variant="h6" gutterBottom>
          Yemekler
        </Typography>

        {formik.values.foods.map((mealFood, index) => (
          <Paper key={index} sx={{ p: 2, mb: 2 }}>
            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2, alignItems: 'center' }}>
              <Box sx={{ flex: '1 1 40%', minWidth: 200 }}>
                <FoodSearch
                  value={mealFood.food}
                  onChange={(food) => handleFoodChange(index, food)}
                  error={
                    formik.touched.foods?.[index]?.food &&
                    Boolean((formik.errors.foods?.[index] as any)?.food)
                  }
                  helperText={
                    formik.touched.foods?.[index]?.food &&
                    (formik.errors.foods?.[index] as any)?.food
                  }
                />
              </Box>
              <Box sx={{ flex: '0 1 15%', minWidth: 120 }}>
                <TextField
                  fullWidth
                  type="number"
                  label="Miktar"
                  value={mealFood.quantity}
                  onChange={(e) => handleQuantityChange(index, Number(e.target.value))}
                  error={
                    formik.touched.foods?.[index]?.quantity &&
                    Boolean((formik.errors.foods?.[index] as any)?.quantity)
                  }
                  helperText={
                    formik.touched.foods?.[index]?.quantity &&
                    (formik.errors.foods?.[index] as any)?.quantity
                  }
                />
              </Box>
              <Box sx={{ flex: '0 1 15%', minWidth: 120 }}>
                <TextField
                  fullWidth
                  select
                  label="Birim"
                  value={mealFood.unit}
                  onChange={(e) => {
                    const newFoods = [...formik.values.foods];
                    newFoods[index] = { ...newFoods[index], unit: e.target.value };
                    formik.setFieldValue('foods', newFoods);
                  }}
                  error={
                    formik.touched.foods?.[index]?.unit &&
                    Boolean((formik.errors.foods?.[index] as any)?.unit)
                  }
                  helperText={
                    formik.touched.foods?.[index]?.unit &&
                    (formik.errors.foods?.[index] as any)?.unit
                  }
                >
                  {UNITS.map((option) => (
                    <MenuItem key={option.value} value={option.value}>
                      {option.label}
                    </MenuItem>
                  ))}
                </TextField>
              </Box>
              <Box sx={{ flex: '0 1 15%', minWidth: 100 }}>
                <Typography variant="body2" color="text.secondary">
                  {mealFood.calories.toFixed(1)} kcal
                </Typography>
              </Box>
              <Box sx={{ flex: '0 0 auto' }}>
                <IconButton
                  color="error"
                  onClick={() => handleRemoveFood(index)}
                  disabled={formik.values.foods.length === 1}
                >
                  <DeleteIcon />
                </IconButton>
              </Box>
            </Box>
          </Paper>
        ))}

        <Button
          startIcon={<AddIcon />}
          onClick={handleAddFood}
          sx={{ mt: 2 }}
        >
          Yemek Ekle
        </Button>
      </Box>

      <Divider sx={{ my: 3 }} />

      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h6">
          Toplam Kalori: {totalCalories.toFixed(1)} kcal
        </Typography>
      </Box>

      <Box sx={{ display: 'flex', gap: 2 }}>
        <Button
          variant="contained"
          color="primary"
          type="submit"
          disabled={loading}
        >
          {initialValues ? 'Güncelle' : 'Ekle'}
        </Button>
        <Button
          variant="outlined"
          onClick={onCancel}
          disabled={loading}
        >
          İptal
        </Button>
      </Box>
    </form>
  );
} 