using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebApplication2.Dto;
using WebApplication2.Exceptions;
using WebApplication2.Services;
using WebApplication2.Services.WebApplication2.Services;

namespace WebApplication2.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class Warehouse2Controller : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public Warehouse2Controller(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterProductWithProcedureAsync([FromBody] RegisterProductInWarehouseRequestDTO dto)
    {
        try
        {
            var idProductWarehouse = await _warehouseService.RegisterProductInWarehouseUsingProcedureAsync(dto);
            return Ok(new { idProductWarehouse });
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
}