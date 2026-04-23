import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginDto } from '../models/login-dto.model';
import { RegisterDto } from '../models/register-dto.model';
import { AuthResponse, ApiResponse } from '../models/auth-response.model';

const TOKEN_KEY = 'hrms_token';
const USER_KEY = 'hrms_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`;

  // --- Reactive state using Angular signals ---
  private _currentUser = signal<AuthResponse | null>(this._loadUserFromStorage());
  
  readonly currentUser = this._currentUser.asReadonly();
  readonly isLoggedIn = computed(() => this._currentUser() !== null);
  readonly userRole = computed(() => this._currentUser()?.role ?? null);
  readonly isAdmin = computed(() => this._currentUser()?.role === 'Admin');
  readonly isHR = computed(() => this._currentUser()?.role === 'HR' || this._currentUser()?.role === 'Admin');

  constructor(private http: HttpClient, private router: Router) {}

  login(dto: LoginDto): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/Login`, dto).pipe(
      tap(response => {
        if (response.success && response.data) {
          this._saveSession(response.data);
        }
      })
    );
  }

  register(dto: RegisterDto): Observable<ApiResponse<object>> {
    return this.http.post<ApiResponse<object>>(`${this.apiUrl}/register`, dto);
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this._currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  private _saveSession(user: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, user.token);
    localStorage.setItem(USER_KEY, JSON.stringify(user));
    this._currentUser.set(user);
  }

  private _loadUserFromStorage(): AuthResponse | null {
    try {
      const raw = localStorage.getItem(USER_KEY);
      if (!raw) return null;
      const user: AuthResponse = JSON.parse(raw);
      // Check if token is still valid (not expired)
      const expiry = new Date(user.expiresAt);
      if (expiry < new Date()) {
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(USER_KEY);
        return null;
      }
      return user;
    } catch {
      return null;
    }
  }
}
