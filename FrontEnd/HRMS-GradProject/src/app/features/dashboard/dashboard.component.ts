import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard-header">
      <h2>Dashboard Overview</h2>
      <p>Welcome to HRMS. Here is an overview of your organization.</p>
    </div>
    
    <div class="stats-grid">
      <div class="stat-card">
        <h3>Total Employees</h3>
        <p class="stat-value">124</p>
      </div>
      <div class="stat-card">
        <h3>Departments</h3>
        <p class="stat-value">8</p>
      </div>
      <div class="stat-card">
        <h3>Pending Leave Requests</h3>
        <p class="stat-value">5</p>
      </div>
      <div class="stat-card">
        <h3>Active Positions</h3>
        <p class="stat-value">24</p>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-header {
      margin-bottom: 2rem;
    }
    .dashboard-header h2 {
      font-size: 1.8rem;
      color: #f1f5f9;
      margin-bottom: 0.5rem;
    }
    .dashboard-header p {
      color: #94a3b8;
    }
    .stats-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
      gap: 1.5rem;
    }
    .stat-card {
      background: #1a1f36;
      border: 1px solid rgba(255, 255, 255, 0.08);
      border-radius: 12px;
      padding: 1.5rem;
      transition: transform 0.2s, box-shadow 0.2s;
    }
    .stat-card:hover {
      transform: translateY(-2px);
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.2);
    }
    .stat-card h3 {
      font-size: 1rem;
      color: #cbd5e1;
      font-weight: 500;
      margin-bottom: 1rem;
    }
    .stat-value {
      font-size: 2.5rem;
      font-weight: 700;
      color: #f1f5f9;
    }
  `]
})
export class DashboardComponent {}
