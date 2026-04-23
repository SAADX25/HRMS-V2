import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DepartmentService } from '../../core/services/department.service';
import { DepartmentDto } from '../../core/models/department.model';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './departments.component.html',
  styleUrl: './departments.component.css'
})
export class DepartmentsComponent implements OnInit {
  private readonly departmentService = inject(DepartmentService);
  private readonly fb = inject(FormBuilder);

  departments = signal<DepartmentDto[]>([]);
  
  isLoading = signal<boolean>(true);
  errorMessage = signal<string | null>(null);

  // Modal & Form State
  isModalOpen = signal<boolean>(false);
  isEditMode = signal<boolean>(false);
  isSubmitting = signal<boolean>(false);
  selectedDepartmentId = signal<number | null>(null);
  
  departmentForm!: FormGroup;

  constructor() {
    this.initForm();
  }

  ngOnInit(): void {
    this.loadDepartments();
  }

  initForm(): void {
    this.departmentForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      location: ['', [Validators.required, Validators.maxLength(150)]]
    });
  }

  loadDepartments(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.departmentService.getAll().subscribe({
      next: (data) => {
        this.departments.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Failed to load departments. Please try again later.');
        this.isLoading.set(false);
        console.error(err);
      }
    });
  }

  onAdd(): void {
    this.isEditMode.set(false);
    this.selectedDepartmentId.set(null);
    this.departmentForm.reset();
    this.isModalOpen.set(true);
  }

  onEdit(id: number): void {
    const department = this.departments().find(d => d.id === id);
    if (!department) return;

    this.isEditMode.set(true);
    this.selectedDepartmentId.set(id);
    
    this.departmentForm.patchValue({
      name: department.name,
      location: department.location
    });

    this.isModalOpen.set(true);
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this department? Warning: This might fail if there are employees assigned to it.')) {
      this.departmentService.delete(id).subscribe({
        next: () => {
          this.departments.update(depts => depts.filter(d => d.id !== id));
        },
        error: (err) => {
          alert('Failed to delete department. Please ensure no employees are assigned to it.');
          console.error(err);
        }
      });
    }
  }

  closeModal(): void {
    this.isModalOpen.set(false);
    this.departmentForm.reset();
  }

  onSubmit(): void {
    if (this.departmentForm.invalid) {
      this.departmentForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    const formValue = this.departmentForm.getRawValue();

    if (this.isEditMode() && this.selectedDepartmentId() !== null) {
      // Update
      const updateDto: Partial<DepartmentDto> = {
        name: formValue.name,
        location: formValue.location
      };

      this.departmentService.update(this.selectedDepartmentId()!, updateDto).subscribe({
        next: (updatedDepartment) => {
          this.departments.update(depts => depts.map(d => d.id === updatedDepartment.id ? updatedDepartment : d));
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
      const createDto: Partial<DepartmentDto> = {
        name: formValue.name,
        location: formValue.location
      };

      this.departmentService.create(createDto).subscribe({
        next: (newDepartment) => {
          this.departments.update(depts => [...depts, newDepartment]);
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
    const field = this.departmentForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched) : false;
  }
}
