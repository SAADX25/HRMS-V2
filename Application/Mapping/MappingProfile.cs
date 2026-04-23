// Aother : Abedalqader Alfaqeeh
// last Edit : 2026-04-12
// </sammer> the MappingProfile class defines how to map between domain entities and data transfer objects (DTOs) using AutoMapper.
// It includes mappings for the Employee entity, allowing for both retrieval (mapping to EmployeeDto) and creation/update (mapping from CreateEmployeeDto and UpdateEmployeeDto).
// The update mapping is configured to ignore null values, enabling partial updates without overwriting existing data with nulls.

using Application.DTOs.Employee;
using Application.DTOs.Departments;
using Application.DTOs.LeaveRequests;
using Application.DTOs.Attendance;
using Application.DTOs.Salary;
using Application.DTOs.Notification;
using AutoMapper;
using Domain.Entities;


namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Employee 
        // For retrieval, we want to map from Employee to EmployeeDto, including the department name
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src => src.Department.Name));


        // For creation, we want to map from CreateEmployeeDto to Employee
        CreateMap<CreateEmployeeDto, Employee>();

        // For update, we want to ignore null values to allow partial updates
        CreateMap<UpdateEmployeeDto, Employee>()
            .ForAllMembers(opt => opt.Condition(
                (src, dest, srcMember) => srcMember != null));  // Ignore null values during update













        // Department Mappings
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>()
            .ForAllMembers(opt => opt.Condition(
                (src, dest, srcMember) => srcMember != null));

        // LeaveRequest Mappings
        CreateMap<LeaveRequest, LeaveRequestDto>();
        CreateMap<CreateLeaveRequestDto, LeaveRequest>();

        // Attendance Mappings
        CreateMap<Attendance, AttendanceDto>();
        CreateMap<CreateAttendanceDto, Attendance>();
        CreateMap<UpdateAttendanceDto, Attendance>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Salary Mappings
        CreateMap<Salary, SalaryDto>();
        CreateMap<CreateSalaryDto, Salary>();

        // Notification Mappings
        CreateMap<Notification, NotificationDto>();
        CreateMap<CreateNotificationDto, Notification>();
    }
}