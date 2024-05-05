using System.Data;
using Microsoft.AspNetCore.Mvc;
using Accountool.Services;
using Accountool.Models;

namespace Accountool.Controllers;
[ApiController]
public partial class EquipmentController : Controller
{
    private readonly IEquipmentService _service;
    public EquipmentController(IEquipmentService service)
    {
        _service = service;
    }

    [HttpGet("Equipment", Name = "GetEquipment")]
    [ProducesResponseType(typeof(IEnumerable<Equipment>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("Equipment/{id:int}", Name = "GetEquipmentById")]
    [ProducesResponseType(typeof(Equipment), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetById(id));
    }

    [HttpPost("Equipment", Name = "CreateEquipment")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Post([FromBody] Equipment entity)
    {
        return Ok(await _service.Create(entity));
    }

    [HttpPut("Equipment", Name = "UpdateEquipment")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Put([FromBody] Equipment entity)
    {
        await _service.Update(entity);
        return Ok();
    }

    [HttpDelete("Equipment/{id:int}", Name = "DeleteEquipment")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }

    [HttpGet("Kiosks/{KioskId}/Equipment", Name = "GetEquipmentForKiosk")]
    [ProducesResponseType(typeof(IEnumerable<Equipment>), 200)]
    public async Task<IActionResult> GetEquipmentForKiosk(int KioskId)
    {
        return Ok(await _service.GetForKiosk(KioskId));
    }
}