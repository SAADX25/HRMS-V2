export enum LeaveType {
  Annual = 0,
  Sick = 1,
  Unpaid = 2
}

export enum LeaveStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2
}

export interface LeaveRequestDto {
  id: number;
  leaveType: LeaveType;
  status: LeaveStatus;
  startDate: string; // ISO date string
  endDate: string; // ISO date string
  rejectionReason?: string | null;
  employeeId: number;
  approvedBy?: number | null;
}

export interface CreateLeaveRequestDto {
  leaveType: LeaveType;
  startDate: string; // ISO date string
  endDate: string; // ISO date string
  employeeId: number;
}
