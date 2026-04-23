import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto } from '../models/employee.model';
import { ApiResponse } from '../models/auth-response.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private readonly apiUrl = `${environment.apiUrl}/Employee`;
  private readonly http = inject(HttpClient);

  getAll(): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(this.apiUrl);
  }

  getById(id: number): Observable<EmployeeDto> {
    return this.http.get<EmployeeDto>(`${this.apiUrl}/${id}`);
  }

  getByDepartment(departmentId: number): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(`${this.apiUrl}/department/${departmentId}`);
  }

  create(dto: CreateEmployeeDto): Observable<EmployeeDto> {
    return this.http.post<EmployeeDto>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdateEmployeeDto): Observable<EmployeeDto> {
    return this.http.put<EmployeeDto>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
