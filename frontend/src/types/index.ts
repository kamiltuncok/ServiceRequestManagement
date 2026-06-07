export interface ServiceRequest {
  id: number;
  customerName: string;
  deviceName: string;
  description: string;
  status: string;
  createdDate: string;
}

export interface ServiceRequestCreateDto {
  customerName: string;
  deviceName: string;
  description: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export interface ApiResult {
  success: boolean;
  message: string;
}
