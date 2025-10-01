import { Snackbar, Alert } from '@mui/material';
import type { AlertColor } from '@mui/material';
import { useState, useCallback } from 'react';

interface NotificationContextType {
  showNotification: (message: string, severity: AlertColor) => void;
}

export const useNotification = () => {
  const [open, setOpen] = useState(false);
  const [message, setMessage] = useState('');
  const [severity, setSeverity] = useState<AlertColor>('info');

  const showNotification = useCallback((message: string, severity: AlertColor) => {
    setMessage(message);
    setSeverity(severity);
    setOpen(true);
  }, []);

  const handleClose = () => {
    setOpen(false);
  };

  return {
    showNotification,
    NotificationComponent: (
      <Snackbar
        open={open}
        autoHideDuration={6000}
        onClose={handleClose}
        anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      >
        <Alert onClose={handleClose} severity={severity} sx={{ width: '100%' }}>
          {message}
        </Alert>
      </Snackbar>
    ),
  };
};

export default useNotification; 