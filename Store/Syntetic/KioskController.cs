using System.Data;
using Microsoft.AspNetCore.Mvc;
using Accountool.Services;
using Accountool.Models;

namespace Accountool.Controllers;
[ApiController]
public partial class KioskController : Controller
{
    private readonly IKioskService _service;
    public KioskController(IKioskService service)
    {
        _service = service;
    }

    [HttpGet("Kiosks", Name = "GetKiosks")]
    [ProducesResponseType(typeof(IEnumerable<Kiosk>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("Kiosks/{id:int}", Name = "GetKioskById")]
    [ProducesResponseType(typeof(Kiosk), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetById(id));
    }

    [HttpPost("Kiosks", Name = "CreateKiosk")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Post([FromBody] Kiosk entity)
    {
        return Ok(await _service.Create(entity));
    }

    [HttpPut("Kiosks", Name = "UpdateKiosk")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Put([FromBody] Kiosk entity)
    {
        await _service.Update(entity);
        return Ok();
    }

    [HttpDelete("Kiosks/{id:int}", Name = "DeleteKiosk")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }

    [HttpGet("Towns/{TownId}/Kiosks", Name = "GetKiosksForTown")]
    [ProducesResponseType(typeof(IEnumerable<Kiosk>), 200)]
    public async Task<IActionResult> GetKiosksForTown(int TownId)
    {
        return Ok(await _service.GetForTown(TownId));
    }

    [HttpGet("Organizations/{OrganizationId}/Kiosks", Name = "GetKiosksForOrganization")]
    [ProducesResponseType(typeof(IEnumerable<Kiosk>), 200)]
    public async Task<IActionResult> GetKiosksForOrganization(int OrganizationId)
    {
        return Ok(await _service.GetForOrganization(OrganizationId));
    }

    [HttpGet("KioskSections/{KioskSectionId}/Kiosks", Name = "GetKiosksForKioskSection")]
    [ProducesResponseType(typeof(IEnumerable<Kiosk>), 200)]
    public async Task<IActionResult> GetKiosksForKioskSection(int KioskSectionId)
    {
        return Ok(await _service.GetForKioskSection(KioskSectionId));
    }
}