using System.Data;
using Microsoft.AspNetCore.Mvc;
using Accountool.Services;
using Accountool.Models;

namespace Accountool.Controllers;
[ApiController]
public partial class KioskSectionController : Controller
{
    private readonly IKioskSectionService _service;
    public KioskSectionController(IKioskSectionService service)
    {
        _service = service;
    }

    [HttpGet("KioskSections", Name = "GetKioskSections")]
    [ProducesResponseType(typeof(IEnumerable<KioskSection>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("KioskSections/{id:int}", Name = "GetKioskSectionById")]
    [ProducesResponseType(typeof(KioskSection), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetById(id));
    }

    [HttpPost("KioskSections", Name = "CreateKioskSection")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Post([FromBody] KioskSection entity)
    {
        return Ok(await _service.Create(entity));
    }

    [HttpPut("KioskSections", Name = "UpdateKioskSection")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Put([FromBody] KioskSection entity)
    {
        await _service.Update(entity);
        return Ok();
    }

    [HttpDelete("KioskSections/{id:int}", Name = "DeleteKioskSection")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }
}