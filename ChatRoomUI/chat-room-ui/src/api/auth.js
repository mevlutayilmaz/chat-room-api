import api from "./axiosConfig";

export const login = async (usernameOrEmail, password) => {
  try {
    return await api.post("/auth/login", { usernameOrEmail, password });
  } catch (error) {
    console.error('API isteği başarısız:', error);  
  }
}

export const signup = async (username, email, password) => {
  try {
    await api.post("/auth/register", { username, email, password }, { successMessage: 'Kayıt başarılı! Bilgilerinizle giriş yapabilirsiniz.' }); 
  } catch (error) {
    console.error('API isteği başarısız:', error);
  }
}