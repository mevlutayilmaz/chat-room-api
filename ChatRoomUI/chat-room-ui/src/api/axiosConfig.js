import axios from 'axios';
import { showErrorToast, showSuccessToast, showWarningToast } from '../utils/toastUtils';
import AuthService from '../services/AuthService';

const BASE_URL = 'https://localhost:7283/api';

const api = axios.create({
  baseURL: BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use(
  async config => {
    if (config.requiresAuth) {
      try {
        const token = AuthService.getAccessToken();
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        } else {
          showWarningToast('Lütfen giriş yapın.');
          return Promise.reject('Token bulunamadı.');
        }
      } catch (error) {
        return Promise.reject(error);
      }
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  response => {
    if (response.config.successMessage) {
      showSuccessToast(response.config.successMessage);
    }
    return response
  },
  error => {
    if (error.response) {
      switch (error.response.status) {
        case 400:
          showWarningToast('Geçersiz istek. Lütfen girdiğiniz bilgileri kontrol edin.');
          break;
        case 401:
          showWarningToast('Yetkisiz erişim!');
          break;
        case 403:
          showWarningToast('Bu işlemi yapmaya yetkiniz yok.');
          break;
        case 404:
          showWarningToast('Aradığınız kaynak bulunamadı.');
          break;
        case 500:
          showErrorToast('Sunucu hatası. Lütfen daha sonra tekrar deneyin.');
          break;
        case 503:
          showErrorToast('Hizmet kullanılamıyor. Lütfen daha sonra tekrar deneyin.');
          break;
        default:
          showErrorToast('Bilinmeyen bir hata oluştu.');
          break;
      }
    } else if (error.request) {
      showErrorToast('Sunucuya ulaşılamıyor. İnternet bağlantınızı kontrol edin.');
    } else {
      showErrorToast('Bir hata oluştu. Lütfen tekrar deneyin.');
    }
    return Promise.reject(error);
  }
);

export default api;