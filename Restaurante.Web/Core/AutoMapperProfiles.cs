using AutoMapper;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Categories;
using Restaurante.Web.DTOs.DiningTables;
using Restaurante.Web.DTOs.Dishes;
using Restaurante.Web.DTOs.Employees;
using Restaurante.Web.DTOs.Roles;
using Restaurante.Web.Data.Enums;
using Restaurante.Web.DTOs.Orders;

namespace Restaurante.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Role, CreateRoleDTO>().ReverseMap();
            CreateMap<Role, UpdateRoleDTO>().ReverseMap();
            CreateMap<DiningTable, DiningTableDTO>().ReverseMap();
            CreateMap<DiningTable, CreateDiningTableDTO>().ReverseMap();
            CreateMap<DiningTable, UpdateDiningTableDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>();

            CreateMap<Employee, CreateEmployeeDTO>().ForMember(d => d.Roles, o => o.Ignore()).ReverseMap();

            CreateMap<Employee, UpdateEmployeeDTO>().ForMember(d => d.Roles, o => o.Ignore()).ReverseMap();

            CreateMap<Dish, DishDTO>();
            CreateMap<Dish, CreateDishDTO>().ForMember(d => d.Categories, o => o.Ignore()).ReverseMap();
            CreateMap<Dish, UpdateDishDTO>().ForMember(d => d.Categories, o => o.Ignore()).ReverseMap();

            CreateMap<OrderDetail, OrderDetailDTO>();

            CreateMap<Order, OrderDTO>()
                .ForMember(d => d.TableNumber, o => o.MapFrom(s => s.DiningTable.Number))
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee.Name))
                .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.Bill != null ? (PaymentMethod?)s.Bill.PaymentMethod : null))
                .ForMember(d => d.PaidAt, o => o.MapFrom(s => s.Bill != null ? (DateTime?)s.Bill.PaidAt : null));
        }
    }
}