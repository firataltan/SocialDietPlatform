import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Typography,
  Button,
  Card,
  CardContent,
  CardActions,
  IconButton,
  Box,
  Chip,
} from '@mui/material';
import { Add as AddIcon, Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { dietPlanService } from '../services/dietPlanService';
import type { DietPlan } from '../types';

export default function DietPlans() {
  const [dietPlans, setDietPlans] = useState<DietPlan[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    loadDietPlans();
  }, []);

  const loadDietPlans = async () => {
    try {
      setLoading(true);
      const response = await dietPlanService.getDietPlans();
      // Ensure response and response.data exist and response.data is an array
      if (response && Array.isArray(response.data)) {
        setDietPlans(response.data);
        setError(null);
      } else {
        // Handle cases where data is not in the expected format
        setDietPlans([]); // Set to empty array to prevent map errors
        setError('Beklenmedik veri formatı.');
        console.error('Unexpected data format for diet plans:', response);
      }
    } catch (err) {
      setError('Diyet planları yüklenirken bir hata oluştu.');
      console.error('Error loading diet plans:', err);
      setDietPlans([]); // Ensure dietPlans is an array even on error
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Bu diyet planını silmek istediğinizden emin misiniz?')) {
      try {
        await dietPlanService.deleteDietPlan(id);
        setDietPlans(dietPlans.filter(plan => plan.id !== id));
      } catch (err) {
        setError('Diyet planı silinirken bir hata oluştu.');
        console.error('Error deleting diet plan:', err);
      }
    }
  };

  if (loading) {
    return (
      <Container>
        <Typography>Yükleniyor...</Typography>
      </Container>
    );
  }

  if (error) {
    return (
      <Container>
        <Typography color="error">{error}</Typography>
      </Container>
    );
  }

  return (
    <Container>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 4 }}>
        <Typography variant="h4" component="h1">
          Diyet Planlarım
        </Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={() => navigate('/diet-plans/new')}
        >
          Yeni Diyet Planı
        </Button>
      </Box>

      {dietPlans && dietPlans.length > 0 ? (
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: { xs: '1fr', sm: 'repeat(auto-fit, minmax(250px, 1fr))' },
            gap: 3,
          }}
        >
          {dietPlans.map((plan) => (
            <Card key={plan.id} sx={{ display: 'flex', flexDirection: 'column' }}>
              <CardContent sx={{ flexGrow: 1 }}>
                <Typography variant="h6" component="h2" gutterBottom>
                  {plan.name}
                </Typography>
                <Typography color="textSecondary" gutterBottom>
                  {plan.description}
                </Typography>
                <Box sx={{ mt: 2 }}>
                  <Chip
                    label={plan.isActive ? 'Aktif' : 'Pasif'}
                    color={plan.isActive ? 'success' : 'default'}
                    size="small"
                    sx={{ mr: 1 }}
                  />
                  <Chip
                    label={`${plan.targetCalories} kcal`}
                    color="primary"
                    size="small"
                  />
                </Box>
                <Typography variant="body2" color="textSecondary" sx={{ mt: 2 }}>
                  {new Date(plan.startDate).toLocaleDateString()} - {new Date(plan.endDate).toLocaleDateString()}
                </Typography>
              </CardContent>
              <CardActions>
                <IconButton
                  size="small"
                  onClick={() => navigate(`/diet-plans/${plan.id}`)}
                >
                  <EditIcon />
                </IconButton>
                <IconButton
                  size="small"
                  color="error"
                  onClick={() => handleDelete(plan.id)}
                >
                  <DeleteIcon />
                </IconButton>
              </CardActions>
            </Card>
          ))}
        </Box>
      ) : (
        <Typography variant="h6" color="text.secondary" align="center">
          Henüz bir diyet planınız yok. Yeni bir plan oluşturarak başlayın!
        </Typography>
      )}
    </Container>
  );
} 