export interface EmployeeDto {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  hireDate: string; // ISO date string
  isActive: boolean;
  departmentId: number;
  departmentName: string;
}

export interface CreateEmployeeDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  hireDate: string; // ISO date string
  departmentId: number;
}

export interface UpdateEmployeeDto {
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  isActive?: boolean;
  departmentId?: number;
}
