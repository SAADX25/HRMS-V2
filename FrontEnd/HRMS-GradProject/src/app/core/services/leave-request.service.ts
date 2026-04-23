import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LeaveRequestDto, CreateLeaveRequestDto, LeaveStatus } from '../models/leave-request.model';

@Injectable({
  providedIn: 'root'
})
export class LeaveRequestService {
  private readonly apiUrl = `${environment.apiUrl}/LeaveRequest`;
  private readonly http = inject(HttpClient);

  getAll(): Observable<LeaveRequestDto[]> {
    return this.http.get<LeaveRequestDto[]>(this.apiUrl);
  }

  getById(id: number): Observable<LeaveRequestDto> {
    return this.http.get<LeaveRequestDto>(`${this.apiUrl}/${id}`);
  }

  getByEmployeeId(employeeId: number): Observable<LeaveRequestDto[]> {
    return this.http.get<LeaveRequestDto[]>(`${this.apiUrl}/employee/${employeeId}`);
  }

  create(dto: CreateLeaveRequestDto): Observable<LeaveRequestDto> {
    return this.http.post<LeaveRequestDto>(this.apiUrl, dto);
  }

  updateStatus(id: number, status: LeaveStatus, rejectionReason?: string): Observable<LeaveRequestDto> {
    return this.http.put<LeaveRequestDto>(`${this.apiUrl}/${id}/status`, { status, rejectionReason });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
