// Matches backend: Application/DTOs/Auth/AuthResponseDto.cs
export interface AuthResponse {
  token: string;
  email: string;
  username: string;
  role: string;
  expiresAt: string; // ISO date string from JSON
}

// Matches backend: Application/Common/ApiResponse<T>
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T | null;
  errors: string[] | null;
}
