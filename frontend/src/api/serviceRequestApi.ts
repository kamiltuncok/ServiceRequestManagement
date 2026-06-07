import axios from 'axios';
import { ApiResponse, ApiResult, ServiceRequest, ServiceRequestCreateDto } from '../types';

const API_BASE_URL = 'http://localhost:5250/api/requests';

const api = axios.create({
  baseURL: API_BASE_URL,
});

export const getRequests = async (searchTerm?: string, status?: string) => {
  const params = new URLSearchParams();
  if (searchTerm) params.append('searchTerm', searchTerm);
  if (status) params.append('status', status);
  
  const response = await api.get<ApiResponse<ServiceRequest[]>>(`?${params.toString()}`);
  return response.data;
};

export const getRequestById = async (id: number) => {
  const response = await api.get<ApiResponse<ServiceRequest>>(`/${id}`);
  return response.data;
};

export const createRequest = async (data: ServiceRequestCreateDto) => {
  const response = await api.post<ApiResult>('', data);
  return response.data;
};

export const updateRequestStatus = async (id: number, status: string) => {
  const response = await api.put<ApiResult>(`/${id}/status`, { status });
  return response.data;
};

export const deleteRequest = async (id: number) => {
  const response = await api.delete<ApiResult>(`/${id}`);
  return response.data;
};
