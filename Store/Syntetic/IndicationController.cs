using System.Data;
using Microsoft.AspNetCore.Mvc;
using Accountool.Services;
using Accountool.Models;

namespace Accountool.Controllers;
[ApiController]
public partial class IndicationController : Controller
{
    private readonly IIndicationService _service;
    public IndicationController(IIndicationService service)
    {
        _service = service;
    }

    [HttpGet("Indications", Name = "GetIndications")]
    [ProducesResponseType(typeof(IEnumerable<Indication>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("Indications/{id:int}", Name = "GetIndicationById")]
    [ProducesResponseType(typeof(Indication), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetById(id));
    }

    [HttpPost("Indications", Name = "CreateIndication")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Post([FromBody] Indication entity)
    {
        return Ok(await _service.Create(entity));
    }

    [HttpPut("Indications", Name = "UpdateIndication")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Put([FromBody] Indication entity)
    {
        await _service.Update(entity);
        return Ok();
    }

    [HttpDelete("Indications/{id:int}", Name = "DeleteIndication")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }

    [HttpGet("Schetchiks/{SchetchikId}/Indications", Name = "GetIndicationsForSchetchik")]
    [ProducesResponseType(typeof(IEnumerable<Indication>), 200)]
    public async Task<IActionResult> GetIndicationsForSchetchik(int SchetchikId)
    {
        return Ok(await _service.GetForSchetchik(SchetchikId));
    }
}