using Application.CQRS.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Management")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("warehouse-status")]
        public async Task<IActionResult> WarehouseStatus([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetWarehouseStatusQuery(pageNumber, pageSize));
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("warehouseItemsStatus")]
        public async Task<IActionResult> GetWarehouseItemsStatus([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sort = "desc")
        {
            var result = await _mediator.Send(new GetWarehouseItemsQuery(pageNumber, pageSize, sort));
            return StatusCode(result.StatusCode, result);
        }
    }
}
