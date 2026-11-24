using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.Core.Attributes;
using Restaurante.Web.Core.Pagination;
using Restaurante.Web.DTOs.DiningTables;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers.Api
{
    [Route("api/diningtables")]
    public class DiningTablesApiController : ApiController
    {
        private readonly IDiningTablesService _service;

        public DiningTablesApiController(IDiningTablesService service)
        {
            _service = service;
        }

        // GET: api/diningtables
        [HttpGet]
        [CustomAuthorize("tables.show")]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationRequest request)
        {
            var response = await _service.GetPaginationAsync(request);
            return ControllerBasicValidation(response);
        }

        // GET: api/diningtables/{id}
        [HttpGet("{id:guid}")]
        [CustomAuthorize("tables.show")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _service.GetOneAsync(id);
            return ControllerBasicValidation(response);
        }

        // POST: api/diningtables
        [HttpPost]
        [CustomAuthorize("tables.create")]
        public async Task<IActionResult> Create(
            [FromBody] CreateDiningTableDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return ControllerBasicValidation(
                    Response<CreateDiningTableDTO>.Failure("Datos inválidos"),
                    ModelState);
            }

            var response = await _service.CreateAsync(dto);

            if (!response.IsSuccess)
                return ControllerBasicValidation(response);

            return Created("/api/diningtables", response);
        }

        // PUT: api/diningtables/{id}
        [HttpPut("{id:guid}")]
        [CustomAuthorize("tables.update")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateDiningTableDTO dto)
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
                    Response<UpdateDiningTableDTO>.Failure("Datos inválidos"),
                    ModelState);
            }

            var response = await _service.UpdateAsync(dto);
            return ControllerBasicValidation(response);
        }

        // DELETE: api/diningtables/{id}
        [HttpDelete("{id:guid}")]
        [CustomAuthorize("tables.delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _service.DeleteAsync(id);
            return ControllerBasicValidation(response);
        }

        // PATCH: api/diningtables/{id}/availability
        [HttpPatch("{id:guid}/availability")]
        [CustomAuthorize("tables.update")]
        public async Task<IActionResult> ToggleAvailability(Guid id)
        {
            var response = await _service.ToggleAvailabilityAsync(id);
            return ControllerBasicValidation(response);
        }
    }
}
