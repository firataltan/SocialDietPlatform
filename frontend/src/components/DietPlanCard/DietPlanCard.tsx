import {
  Card,
  CardContent,
  CardActions,
  Typography,
  Button,
  Chip,
  Box,
  Stack,
} from '@mui/material';
import { AccessTime, Restaurant, FitnessCenter } from '@mui/icons-material';

interface DietPlanCardProps {
  title: string;
  description: string;
  duration: string;
  mealCount: number;
  calories: number;
  onView: () => void;
  onEdit?: () => void;
  onDelete?: () => void;
}

const DietPlanCard = ({
  title,
  description,
  duration,
  mealCount,
  calories,
  onView,
  onEdit,
  onDelete,
}: DietPlanCardProps) => {
  return (
    <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      <CardContent sx={{ flexGrow: 1 }}>
        <Typography gutterBottom variant="h5" component="h2">
          {title}
        </Typography>
        <Typography variant="body2" color="text.secondary" paragraph>
          {description}
        </Typography>
        <Stack direction="row" spacing={1} flexWrap="wrap" gap={1}>
          <Chip
            icon={<AccessTime />}
            label={duration}
            size="small"
            variant="outlined"
          />
          <Chip
            icon={<Restaurant />}
            label={`${mealCount} öğün`}
            size="small"
            variant="outlined"
          />
          <Chip
            icon={<FitnessCenter />}
            label={`${calories} kalori`}
            size="small"
            variant="outlined"
          />
        </Stack>
      </CardContent>
      <CardActions>
        <Button size="small" onClick={onView}>
          Görüntüle
        </Button>
        {onEdit && (
          <Button size="small" onClick={onEdit}>
            Düzenle
          </Button>
        )}
        {onDelete && (
          <Button size="small" color="error" onClick={onDelete}>
            Sil
          </Button>
        )}
      </CardActions>
    </Card>
  );
};

export default DietPlanCard; 