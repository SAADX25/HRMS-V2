import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LeaveRequestService } from '../../core/services/leave-request.service';
import { EmployeeService } from '../../core/services/employee.service';
import { LeaveRequestDto, CreateLeaveRequestDto, LeaveType, LeaveStatus } from '../../core/models/leave-request.model';
import { EmployeeDto } from '../../core/models/employee.model';

@Component({
  selector: 'app-leave-requests',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './leave-requests.component.html',
  styleUrl: './leave-requests.component.css'
})
export class LeaveRequestsComponent implements OnInit {
  private readonly leaveRequestService = inject(LeaveRequestService);
  private readonly employeeService = inject(EmployeeService);
  private readonly fb = inject(FormBuilder);

  // Expose Enums to template
  readonly LeaveType = LeaveType;
  readonly LeaveStatus = LeaveStatus;

  leaveRequests = signal<LeaveRequestDto[]>([]);
  employees = signal<EmployeeDto[]>([]);
  
  isLoading = signal<boolean>(true);
  errorMessage = signal<string | null>(null);

  // Modal & Form State
  isModalOpen = signal<boolean>(false);
  isSubmitting = signal<boolean>(false);
  
  requestForm!: FormGroup;

  constructor() {
    this.initForm();
  }

  ngOnInit(): void {
    this.loadEmployees();
    this.loadLeaveRequests();
  }

  initForm(): void {
    this.requestForm = this.fb.group({
      employeeId: ['', [Validators.required]],
      leaveType: ['', [Validators.required]],
      startDate: ['', [Validators.required]],
      endDate: ['', [Validators.required]]
    });
  }

  loadEmployees(): void {
    this.employeeService.getAll().subscribe({
      next: (data) => this.employees.set(data),
      error: (err) => console.error('Failed to load employees', err)
    });
  }

  loadLeaveRequests(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.leaveRequestService.getAll().subscribe({
      next: (data) => {
        this.leaveRequests.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Failed to load leave requests. Please try again later.');
        this.isLoading.set(false);
        console.error(err);
      }
    });
  }

  getEmployeeName(employeeId: number): string {
    const emp = this.employees().find(e => e.id === employeeId);
    return emp ? (emp.fullName || emp.firstName + ' ' + emp.lastName) : `Emp #${employeeId}`;
  }

  getLeaveTypeName(type: LeaveType): string {
    switch (type) {
      case LeaveType.Annual: return 'Annual';
      case LeaveType.Sick: return 'Sick';
      case LeaveType.Unpaid: return 'Unpaid';
      default: return 'Unknown';
    }
  }

  onAdd(): void {
    this.requestForm.reset();
    
    // Set default dates
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    
    this.requestForm.patchValue({ 
      startDate: today.toISOString().split('T')[0],
      endDate: tomorrow.toISOString().split('T')[0]
    });
    
    this.isModalOpen.set(true);
  }

  onApprove(id: number): void {
    if (confirm('Are you sure you want to approve this leave request?')) {
      this.updateStatus(id, LeaveStatus.Approved);
    }
  }

  onReject(id: number): void {
    const reason = prompt('Please enter a reason for rejection (optional):');
    if (reason !== null) { // User didn't cancel
      this.updateStatus(id, LeaveStatus.Rejected, reason);
    }
  }

  private updateStatus(id: number, status: LeaveStatus, reason?: string): void {
    this.leaveRequestService.updateStatus(id, status, reason).subscribe({
      next: (updatedRequest) => {
        // Update the signal array
        this.leaveRequests.update(requests => 
          requests.map(r => r.id === id ? updatedRequest : r)
        );
      },
      error: (err) => {
        alert('Failed to update leave request status.');
        console.error(err);
      }
    });
  }

  closeModal(): void {
    this.isModalOpen.set(false);
    this.requestForm.reset();
  }

  onSubmit(): void {
    if (this.requestForm.invalid) {
      this.requestForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    const formValue = this.requestForm.getRawValue();

    const createDto: CreateLeaveRequestDto = {
      employeeId: Number(formValue.employeeId),
      leaveType: Number(formValue.leaveType),
      startDate: new Date(formValue.startDate).toISOString(),
      endDate: new Date(formValue.endDate).toISOString()
    };

    this.leaveRequestService.create(createDto).subscribe({
      next: (newRequest) => {
        // Enforce default status as Pending if not returned properly
        if (newRequest.status === undefined) newRequest.status = LeaveStatus.Pending;
        this.leaveRequests.update(requests => [newRequest, ...requests]);
        this.closeModal();
        this.isSubmitting.set(false);
      },
      error: (err) => {
        console.error('Create failed', err);
        this.isSubmitting.set(false);
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.requestForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched) : false;
  }
}
