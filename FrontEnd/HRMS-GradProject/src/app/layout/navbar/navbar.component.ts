import { Component, Output, EventEmitter, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  @Output() toggleSidebar = new EventEmitter<void>();
  
  authService = inject(AuthService);

  onToggle(): void {
    this.toggleSidebar.emit();
  }

  onLogout(): void {
    this.authService.logout();
  }
}
