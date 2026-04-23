import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DepartmentDto } from '../models/department.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private readonly apiUrl = `${environment.apiUrl}/Department`;
  private readonly http = inject(HttpClient);

  getAll(): Observable<DepartmentDto[]> {
    return this.http.get<DepartmentDto[]>(this.apiUrl);
  }

  getById(id: number): Observable<DepartmentDto> {
    return this.http.get<DepartmentDto>(`${this.apiUrl}/${id}`);
  }

  create(dto: Partial<DepartmentDto>): Observable<DepartmentDto> {
    return this.http.post<DepartmentDto>(this.apiUrl, dto);
  }

  update(id: number, dto: Partial<DepartmentDto>): Observable<DepartmentDto> {
    return this.http.put<DepartmentDto>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
