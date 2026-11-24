using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Attributes;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.Dishes;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers.Api
{
    [Route("api/dishes")]
    public class DishesApiController : ApiController
    {
        private readonly IDishesService _service;

        public DishesApiController(IDishesService service)
        {
            _service = service;
        }

        // GET: api/dishes
        [HttpGet]
        [CustomAuthorize("dishes.show")]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationRequest request)
        {
            var response = await _service.GetPaginationAsync(request);

            return ControllerBasicValidation(response);
        }

        // GET: api/dishes/{id}
        [HttpGet("{id:guid}")]
        [CustomAuthorize("dishes.show")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _service.GetOneAsync(id);

            return ControllerBasicValidation(response);
        }

        // POST: api/dishes
        [HttpPost]
        [CustomAuthorize("dishes.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateDishDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return ControllerBasicValidation(
                    Response<CreateDishDTO>.Failure("Datos inválidos"),
                    ModelState);
            }

            var response = await _service.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                return ControllerBasicValidation(response);
            }

            return Created("/api/dishes", response);
        }

        // PUT: api/dishes/{id}
        [HttpPut("{id:guid}")]
        [CustomAuthorize("dishes.update")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateDishDTO dto)
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
                    Response<UpdateDishDTO>.Failure("Datos inválidos"),
                    ModelState);
            }

            var response = await _service.UpdateAsync(dto);

            return ControllerBasicValidation(response);
        }

        // DELETE: api/dishes/{id}
        [HttpDelete("{id:guid}")]
        [CustomAuthorize("dishes.delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _service.DeleteAsync(id);

            return ControllerBasicValidation(response);
        }
    }
}
