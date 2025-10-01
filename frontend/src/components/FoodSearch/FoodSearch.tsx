import { useState, useEffect } from 'react';
import {
  Autocomplete,
  TextField,
  CircularProgress,
  Box,
  Typography,
  Paper,
} from '@mui/material';
import { dietPlanService } from '../../services/dietPlanService';
import type { Food } from '../../types';

interface FoodSearchProps {
  value: Food | null;
  onChange: (food: Food | null) => void;
  label?: string;
  error?: boolean;
  helperText?: string;
}

export default function FoodSearch({ value, onChange, label = 'Yemek Ara', error, helperText }: FoodSearchProps) {
  const [open, setOpen] = useState(false);
  const [options, setOptions] = useState<Food[]>([]);
  const [inputValue, setInputValue] = useState('');
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    let active = true;

    const searchFoods = async () => {
      if (inputValue.length < 2) {
        setOptions([]);
        return;
      }

      setLoading(true);
      try {
        const results = await dietPlanService.searchFoods(inputValue);
        if (active) {
          setOptions(results);
        }
      } catch (error) {
        console.error('Error searching foods:', error);
        if (active) {
          setOptions([]);
        }
      } finally {
        if (active) {
          setLoading(false);
        }
      }
    };

    const timeoutId = setTimeout(searchFoods, 300);
    return () => {
      active = false;
      clearTimeout(timeoutId);
    };
  }, [inputValue]);

  return (
    <Autocomplete
      open={open}
      onOpen={() => setOpen(true)}
      onClose={() => setOpen(false)}
      value={value}
      onChange={(_, newValue) => {
        onChange(newValue);
      }}
      onInputChange={(_, newInputValue) => {
        setInputValue(newInputValue);
      }}
      isOptionEqualToValue={(option, value) => option.id === value.id}
      getOptionLabel={(option) => option.name}
      options={options}
      loading={loading}
      renderInput={(params) => (
        <TextField
          {...params}
          label={label}
          error={error}
          helperText={helperText}
          InputProps={{
            ...params.InputProps,
            endAdornment: (
              <>
                {loading ? <CircularProgress color="inherit" size={20} /> : null}
                {params.InputProps.endAdornment}
              </>
            ),
          }}
        />
      )}
      renderOption={(props, option) => (
        <Paper component="li" {...props}>
          <Box>
            <Typography variant="body1">{option.name}</Typography>
            <Typography variant="body2" color="text.secondary">
              {option.calories} kcal | {option.protein}g protein | {option.carbs}g carbs | {option.fat}g fat
            </Typography>
          </Box>
        </Paper>
      )}
      noOptionsText="Yemek bulunamadı"
      loadingText="Yükleniyor..."
    />
  );
} 