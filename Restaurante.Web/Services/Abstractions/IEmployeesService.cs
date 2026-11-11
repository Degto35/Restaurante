using Restaurante.Web.Core;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Employees;

namespace Restaurante.Web.Services.Abstractions
{
    public interface IEmployeesService
    {
        Task<Response<PaginationResponse<EmployeeDTO>>> GetPaginationAsync(PaginationRequest request);
        Task<Response<EmployeeDTO>> GetOneAsync(Guid id);
        Task<Response<CreateEmployeeDTO>> CreateAsync(CreateEmployeeDTO dto);
        Task<Response<UpdateEmployeeDTO>> UpdateAsync(UpdateEmployeeDTO dto);
        Task<Response> DeleteAsync(Guid id);
    }
}