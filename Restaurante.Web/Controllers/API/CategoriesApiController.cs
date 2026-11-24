using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Attributes;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Categories;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers.Api
{
    [Route("api/categories")]
    public class CategoriesApiController : ApiController
    {
        private readonly ICategoriesService _service;

        public CategoriesApiController(ICategoriesService service)
        {
            _service = service;
        }

        [HttpGet]
        [CustomAuthorize("categories.show")]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationRequest request)
        {
            var response = await _service.GetPaginationAsync(request);
            return ControllerBasicValidation(response);
        }

        [HttpGet("{id:guid}")]
        [CustomAuthorize("categories.show")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _service.GetOneAsync(id);
            return ControllerBasicValidation(response);
        }

        [HttpPost]
        [CustomAuthorize("categories.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return ControllerBasicValidation(
                    Response<CreateCategoryDTO>.Failure("Datos inválidos"),
                    ModelState);
            }

            var response = await _service.CreateAsync(dto);
            return ControllerBasicValidation(response);
        }

        [HttpPut("{id:guid}")]
        [CustomAuthorize("categories.update")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateCategoryDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(
                    Restaurante.Web.Core.Response.Failure(
                        "El identificador no coincide."));
            }

            if (!ModelState.IsValid)
            {
                return ControllerBasicValidation(
                    Response<UpdateCategoryDTO>.Failure("Datos inválidos"),
                    ModelState);
            }

            var response = await _service.UpdateAsync(dto);
            return ControllerBasicValidation(response);
        }

        [HttpDelete("{id:guid}")]
        [CustomAuthorize("categories.delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _service.DeleteAsync(id);
            return ControllerBasicValidation(response);
        }
    }
}
