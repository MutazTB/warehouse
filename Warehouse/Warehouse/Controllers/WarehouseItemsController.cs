using Application.CQRS.WarehouseItems.Commands;
using Application.CQRS.WarehouseItems.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarehouseItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehouseItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] WarehouseItemCreateDto dto)
        {
            var result = await _mediator.Send(new CreateWarehouseItemCommand(dto));
            return Ok(new { message = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetWarehouseItemsQuery(pageNumber, pageSize));
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] WarehouseItemCreateDto dto)
        {
            var result = await _mediator.Send(new UpdateWarehouseItemCommand(id, dto));
            return Ok(new { message = result });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteWarehouseItemCommand(id));
            return Ok(new { message = result });
        }
    }
}
