import api from "./axiosConfig";

export const getAllUsers = async () => {
  try {
    return await api.get("/users/getAllUsers", { requiresAuth: true });
  } catch (error) {
    console.error('API isteği başarısız:', error);  
  }
}