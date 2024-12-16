import api from "./axiosConfig";

export const getMessagesByChatId = async (id) => {
  try {
    const response = await api.get(`/messages/GetMessagesByChatId/${id}`, { requiresAuth: true });
    return response.data;
  } catch (error) {
    console.error('API isteği başarısız:', error);  
  }
}