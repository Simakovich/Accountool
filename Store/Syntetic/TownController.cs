using System.Data;
using Microsoft.AspNetCore.Mvc;
using Accountool.Services;
using Accountool.Models;

namespace Accountool.Controllers;
[ApiController]
public partial class TownController : Controller
{
    private readonly ITownService _service;
    public TownController(ITownService service)
    {
        _service = service;
    }

    [HttpGet("Towns", Name = "GetTowns")]
    [ProducesResponseType(typeof(IEnumerable<Town>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("Towns/{id:int}", Name = "GetTownById")]
    [ProducesResponseType(typeof(Town), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetById(id));
    }

    [HttpPost("Towns", Name = "CreateTown")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Post([FromBody] Town entity)
    {
        return Ok(await _service.Create(entity));
    }

    [HttpPut("Towns", Name = "UpdateTown")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Put([FromBody] Town entity)
    {
        await _service.Update(entity);
        return Ok();
    }

    [HttpDelete("Towns/{id:int}", Name = "DeleteTown")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }
}