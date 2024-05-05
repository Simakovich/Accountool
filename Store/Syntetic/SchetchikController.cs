using System.Data;
using Microsoft.AspNetCore.Mvc;
using Accountool.Services;
using Accountool.Models;

namespace Accountool.Controllers;
[ApiController]
public partial class SchetchikController : Controller
{
    private readonly ISchetchikService _service;
    public SchetchikController(ISchetchikService service)
    {
        _service = service;
    }

    [HttpGet("Schetchiks", Name = "GetSchetchiks")]
    [ProducesResponseType(typeof(IEnumerable<Schetchik>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("Schetchiks/{id:int}", Name = "GetSchetchikById")]
    [ProducesResponseType(typeof(Schetchik), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetById(id));
    }

    [HttpPost("Schetchiks", Name = "CreateSchetchik")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Post([FromBody] Schetchik entity)
    {
        return Ok(await _service.Create(entity));
    }

    [HttpPut("Schetchiks", Name = "UpdateSchetchik")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Put([FromBody] Schetchik entity)
    {
        await _service.Update(entity);
        return Ok();
    }

    [HttpDelete("Schetchiks/{id:int}", Name = "DeleteSchetchik")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }

    [HttpGet("Kiosks/{KioskId}/Schetchiks", Name = "GetSchetchiksForKiosk")]
    [ProducesResponseType(typeof(IEnumerable<Schetchik>), 200)]
    public async Task<IActionResult> GetSchetchiksForKiosk(int KioskId)
    {
        return Ok(await _service.GetForKiosk(KioskId));
    }
}