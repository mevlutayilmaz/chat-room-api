import { toast } from 'react-toastify';

const defaultToastOptions = {
  position: "top-right",
  autoClose: 3000,
  hideProgressBar: true,
  closeOnClick: true,
  pauseOnHover: true,
  draggable: true,
  progress: undefined,
  theme: "colored",
};

export const showSuccessToast = (message, options = {}) => {
  toast.success(message, { ...defaultToastOptions, ...options });
};

export const showErrorToast = (message, options = {}) => {
  toast.error(message, { ...defaultToastOptions, ...options });
};

export const showInfoToast = (message, options = {}) => {
  toast.info(message, { ...defaultToastOptions, ...options });
};

export const showWarningToast = (message, options = {}) => {
  toast.warning(message, { ...defaultToastOptions, ...options });
};