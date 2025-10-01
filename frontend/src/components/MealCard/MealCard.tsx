import {
  Card,
  CardContent,
  CardMedia,
  Typography,
  Box,
  Chip,
  Stack,
} from '@mui/material';
import { Restaurant, AccessTime, LocalDining } from '@mui/icons-material';

interface MealCardProps {
  title: string;
  description: string;
  imageUrl?: string;
  calories: number;
  preparationTime: string;
  mealType: string;
  onSelect?: () => void;
}

const MealCard = ({
  title,
  description,
  imageUrl,
  calories,
  preparationTime,
  mealType,
  onSelect,
}: MealCardProps) => {
  return (
    <Card
      sx={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        cursor: onSelect ? 'pointer' : 'default',
        '&:hover': onSelect
          ? {
              boxShadow: 6,
              transform: 'translateY(-4px)',
              transition: 'all 0.3s ease-in-out',
            }
          : {},
      }}
      onClick={onSelect}
    >
      {imageUrl && (
        <CardMedia
          component="img"
          height="140"
          image={imageUrl}
          alt={title}
        />
      )}
      <CardContent sx={{ flexGrow: 1 }}>
        <Typography gutterBottom variant="h6" component="h3">
          {title}
        </Typography>
        <Typography variant="body2" color="text.secondary" paragraph>
          {description}
        </Typography>
        <Stack direction="row" spacing={1} flexWrap="wrap" gap={1}>
          <Chip
            icon={<Restaurant />}
            label={`${calories} kalori`}
            size="small"
            variant="outlined"
          />
          <Chip
            icon={<AccessTime />}
            label={preparationTime}
            size="small"
            variant="outlined"
          />
          <Chip
            icon={<LocalDining />}
            label={mealType}
            size="small"
            variant="outlined"
          />
        </Stack>
      </CardContent>
    </Card>
  );
};

export default MealCard; 