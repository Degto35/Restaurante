using AutoMapper;
using Restaurante.Web.Data.Entities;
using Restaurante.Web.DTOs.Categories;
using Restaurante.Web.DTOs.Roles;

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
        }
    }
}