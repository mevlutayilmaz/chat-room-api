import api from "./axiosConfig";

export const getAllChats = async () => {
  try {
    const response = await api.get("/chatRooms/getAllChats", { requiresAuth: true });
    return response.data;
  } catch (error) {
    console.error('API isteği başarısız:', error);  
  }
}