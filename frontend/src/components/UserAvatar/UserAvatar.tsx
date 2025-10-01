import { Avatar } from '@mui/material';
import type { AvatarProps } from '@mui/material';
import { Person as PersonIcon } from '@mui/icons-material';

interface UserAvatarProps extends Omit<AvatarProps, 'src'> {
  src?: string;
  name?: string;
  size?: number;
}

const UserAvatar = ({ src, name, size = 40, ...props }: UserAvatarProps) => {
  const getInitials = (name: string) => {
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase();
  };

  return (
    <Avatar
      src={src}
      alt={name}
      sx={{ width: size, height: size, ...props.sx }}
      {...props}
    >
      {name ? getInitials(name) : <PersonIcon />}
    </Avatar>
  );
};

export default UserAvatar; 