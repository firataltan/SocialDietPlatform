import { Pagination as MuiPagination, Box } from '@mui/material';

interface PaginationProps {
  count: number;
  page: number;
  onChange: (page: number) => void;
  size?: 'small' | 'medium' | 'large';
  color?: 'primary' | 'secondary' | 'standard';
}

const Pagination = ({
  count,
  page,
  onChange,
  size = 'medium',
  color = 'primary',
}: PaginationProps) => {
  const handleChange = (_event: React.ChangeEvent<unknown>, value: number) => {
    onChange(value);
  };

  return (
    <Box display="flex" justifyContent="center" my={2}>
      <MuiPagination
        count={count}
        page={page}
        onChange={handleChange}
        size={size}
        color={color}
        showFirstButton
        showLastButton
      />
    </Box>
  );
};

export default Pagination; 