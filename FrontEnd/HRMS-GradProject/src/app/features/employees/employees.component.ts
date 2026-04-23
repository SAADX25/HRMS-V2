import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeeService } from '../../core/services/employee.service';
import { DepartmentService } from '../../core/services/department.service';
import { EmployeeDto, CreateEmployeeDto, UpdateEmployeeDto } from '../../core/models/employee.model';
import { DepartmentDto } from '../../core/models/department.model';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.css'
})
export class EmployeesComponent implements OnInit {
  private readonly employeeService = inject(EmployeeService);
  private readonly departmentService = inject(DepartmentService);
  private readonly fb = inject(FormBuilder);

  employees = signal<EmployeeDto[]>([]);
  departments = signal<DepartmentDto[]>([]);
  
  isLoading = signal<boolean>(true);
  errorMessage = signal<string | null>(null);

  // Modal & Form State
  isModalOpen = signal<boolean>(false);
  isEditMode = signal<boolean>(false);
  isSubmitting = signal<boolean>(false);
  selectedEmployeeId = signal<number | null>(null);
  
  employeeForm!: FormGroup;

  constructor() {
    this.initForm();
  }

  ngOnInit(): void {
    this.loadDepartments();
    this.loadEmployees();
  }

  initForm(): void {
    this.employeeForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9+() -]*$')]],
      hireDate: ['', [Validators.required]],
      departmentId: ['', [Validators.required]],
      isActive: [true]
    });
  }

  loadDepartments(): void {
    this.departmentService.getAll().subscribe({
      next: (data) => this.departments.set(data),
      error: (err) => console.error('Failed to load departments', err)
    });
  }

  loadEmployees(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.employeeService.getAll().subscribe({
      next: (data) => {
        this.employees.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Failed to load employees. Please try again later.');
        this.isLoading.set(false);
        console.error(err);
      }
    });
  }

  onAdd(): void {
    this.isEditMode.set(false);
    this.selectedEmployeeId.set(null);
    this.employeeForm.reset({ isActive: true });
    
    // Set default hire date to today
    const today = new Date().toISOString().split('T')[0];
    this.employeeForm.patchValue({ hireDate: today });
    
    // Disable email field on edit, but ensure it's enabled on add
    this.employeeForm.get('email')?.enable();
    
    this.isModalOpen.set(true);
  }

  onEdit(id: number): void {
    const employee = this.employees().find(e => e.id === id);
    if (!employee) return;

    this.isEditMode.set(true);
    this.selectedEmployeeId.set(id);
    
    // Format date for the input type="date"
    const formattedDate = employee.hireDate ? employee.hireDate.split('T')[0] : '';

    this.employeeForm.patchValue({
      firstName: employee.firstName,
      lastName: employee.lastName,
      email: employee.email,
      phoneNumber: employee.phoneNumber,
      hireDate: formattedDate,
      departmentId: employee.departmentId,
      isActive: employee.isActive
    });

    // Often emails cannot be changed, or if they can, we leave it enabled. We'll leave it enabled for now.
    
    this.isModalOpen.set(true);
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.delete(id).subscribe({
        next: () => {
          this.employees.update(emps => emps.filter(e => e.id !== id));
        },
        error: (err) => {
          alert('Failed to delete employee.');
          console.error(err);
        }
      });
    }
  }

  closeModal(): void {
    this.isModalOpen.set(false);
    this.employeeForm.reset();
  }

  onSubmit(): void {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    const formValue = this.employeeForm.getRawValue();

    if (this.isEditMode() && this.selectedEmployeeId() !== null) {
      // Update
      const updateDto: UpdateEmployeeDto = {
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        phoneNumber: formValue.phoneNumber,
        isActive: formValue.isActive,
        departmentId: Number(formValue.departmentId)
      };

      this.employeeService.update(this.selectedEmployeeId()!, updateDto).subscribe({
        next: (updatedEmployee) => {
          // Find department name manually to update the UI instantly
          const dept = this.departments().find(d => d.id === updatedEmployee.departmentId);
          updatedEmployee.departmentName = dept?.name || '';
          
          this.employees.update(emps => emps.map(e => e.id === updatedEmployee.id ? updatedEmployee : e));
          this.closeModal();
          this.isSubmitting.set(false);
        },
        error: (err) => {
          console.error('Update failed', err);
          this.isSubmitting.set(false);
        }
      });
    } else {
      // Create
      const createDto: CreateEmployeeDto = {
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        email: formValue.email,
        phoneNumber: formValue.phoneNumber,
        hireDate: new Date(formValue.hireDate).toISOString(),
        departmentId: Number(formValue.departmentId)
      };

      this.employeeService.create(createDto).subscribe({
        next: (newEmployee) => {
          // Provide department name locally if API doesn't return it mapped
          const dept = this.departments().find(d => d.id === newEmployee.departmentId);
          newEmployee.departmentName = dept?.name || '';
          // Force isActive to true since default creation is usually active
          newEmployee.isActive = formValue.isActive;

          this.employees.update(emps => [...emps, newEmployee]);
          this.closeModal();
          this.isSubmitting.set(false);
        },
        error: (err) => {
          console.error('Create failed', err);
          this.isSubmitting.set(false);
        }
      });
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.employeeForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched) : false;
  }
}
