// Matches backend: Application/DTOs/Auth/RegisterDto.cs
// UserRole enum: Admin = 0, HR = 1, Employee = 2
export interface RegisterDto {
  username: string;
  email: string;
  password: string;
  role: UserRole;
  employeeId?: number;
}

export enum UserRole {
  Admin = 0,
  HR = 1,
  Employee = 2
}
