import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useFormik } from 'formik';
import * as yup from 'yup';
import {
  Container,
  Typography,
  TextField,
  Button,
  Box,
  FormControlLabel,
  Switch,
  Alert,
  Paper,
  InputAdornment,
  useTheme,
  useMediaQuery,
} from '@mui/material';
import {
  Assignment,
  Description,
  CalendarToday,
  Fastfood,
  Save,
  Cancel,
} from '@mui/icons-material';
import { dietPlanService } from '../services/dietPlanService';
import type { DietPlan } from '../types';

const validationSchema = yup.object({
  name: yup.string().required('Diyet planı adı zorunludur'),
  description: yup.string().required('Açıklama zorunludur'),
  startDate: yup.date().required('Başlangıç tarihi zorunludur'),
  endDate: yup.date()
    .required('Bitiş tarihi zorunludur')
    .min(yup.ref('startDate'), 'Bitiş tarihi başlangıç tarihinden sonra olmalıdır'),
  targetCalories: yup.number()
    .required('Hedef kalori zorunludur')
    .min(0, 'Hedef kalori 0\'dan büyük olmalıdır'),
});

export default function DietPlanForm() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [initialValues, setInitialValues] = useState<Partial<DietPlan>>({
    name: '',
    description: '',
    startDate: '',
    endDate: '',
    targetCalories: 2000,
    isActive: true,
  });

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  useEffect(() => {
    if (id) {
      loadDietPlan();
    }
  }, [id]);

  const loadDietPlan = async () => {
    try {
      setLoading(true);
      const data = await dietPlanService.getDietPlan(id!);
      setInitialValues({
        name: data.name,
        description: data.description,
        startDate: data.startDate.split('T')[0],
        endDate: data.endDate.split('T')[0],
        targetCalories: data.targetCalories,
        isActive: data.isActive,
      });
    } catch (err) {
      setError('Diyet planı yüklenirken bir hata oluştu.');
      console.error('Error loading diet plan:', err);
    } finally {
      setLoading(false);
    }
  };

  const formik = useFormik({
    initialValues,
    validationSchema,
    enableReinitialize: true,
    onSubmit: async (values) => {
      try {
        setLoading(true);
        setError(null);
        if (id) {
          await dietPlanService.updateDietPlan(id, values);
        } else {
          const createPayload: Omit<DietPlan, 'id' | 'meals' | 'createdAt' | 'updatedAt'> = {
            name: values.name as string,
            description: values.description as string,
            startDate: values.startDate as string,
            endDate: values.endDate as string,
            targetCalories: values.targetCalories as number,
            isActive: values.isActive as boolean,
          };
          await dietPlanService.createDietPlan(createPayload);
        }
        navigate('/diet-plans');
      } catch (err) {
        setError('Diyet planı kaydedilirken bir hata oluştu.');
        console.error('Error saving diet plan:', err);
      } finally {
        setLoading(false);
      }
    },
  });

  if (loading && id) {
    return (
      <Container>
        <Typography>Yükleniyor...</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <Paper elevation={3} sx={{ p: { xs: 3, sm: 6 }, borderRadius: 2, bgcolor: 'background.paper' }}>
        <Box sx={{ textAlign: 'center', mb: 4 }}>
          <Typography variant={isMobile ? 'h4' : 'h3'} component="h1" gutterBottom sx={{ fontWeight: 'bold', color: 'primary.main' }}>
            {id ? 'Diyet Planını Düzenle' : 'Yeni Diyet Planı'}
          </Typography>
        </Box>

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <Box component="form" onSubmit={formik.handleSubmit} sx={{ width: '100%' }}>
          <TextField
            fullWidth
            id="name"
            name="name"
            label="Diyet Planı Adı"
            value={formik.values.name}
            onChange={formik.handleChange}
            error={formik.touched.name && Boolean(formik.errors.name)}
            helperText={formik.touched.name && formik.errors.name}
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
            rows={4}
            value={formik.values.description}
            onChange={formik.handleChange}
            error={formik.touched.description && Boolean(formik.errors.description)}
            helperText={formik.touched.description && formik.errors.description}
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
            id="startDate"
            name="startDate"
            label="Başlangıç Tarihi"
            type="date"
            value={formik.values.startDate}
            onChange={formik.handleChange}
            error={formik.touched.startDate && Boolean(formik.errors.startDate)}
            helperText={formik.touched.startDate && formik.errors.startDate}
            margin="normal"
            InputLabelProps={{ shrink: true }}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <CalendarToday color="action" />
                </InputAdornment>
              ),
            }}
          />

          <TextField
            fullWidth
            id="endDate"
            name="endDate"
            label="Bitiş Tarihi"
            type="date"
            value={formik.values.endDate}
            onChange={formik.handleChange}
            error={formik.touched.endDate && Boolean(formik.errors.endDate)}
            helperText={formik.touched.endDate && formik.errors.endDate}
            margin="normal"
            InputLabelProps={{ shrink: true }}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <CalendarToday color="action" />
                </InputAdornment>
              ),
            }}
          />

          <TextField
            fullWidth
            id="targetCalories"
            name="targetCalories"
            label="Hedef Kalori"
            type="number"
            value={formik.values.targetCalories}
            onChange={formik.handleChange}
            error={formik.touched.targetCalories && Boolean(formik.errors.targetCalories)}
            helperText={formik.touched.targetCalories && formik.errors.targetCalories}
            margin="normal"
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <Fastfood color="action" />
                </InputAdornment>
              ),
            }}
          />

          <FormControlLabel
            control={
              <Switch
                checked={formik.values.isActive}
                onChange={formik.handleChange}
                name="isActive"
              />
            }
            label="Aktif"
            sx={{ mt: 2 }}
          />

          <Box sx={{ mt: 3, display: 'flex', gap: 2, justifyContent: 'center' }}>
            <Button
              variant="contained"
              color="primary"
              type="submit"
              disabled={loading}
              startIcon={<Save />}
            >
              {id ? 'Güncelle' : 'Oluştur'}
            </Button>
            <Button
              variant="outlined"
              onClick={() => navigate('/diet-plans')}
              disabled={loading}
              startIcon={<Cancel />}
            >
              İptal
            </Button>
          </Box>
        </Box>
      </Paper>
    </Container>
  );
} 