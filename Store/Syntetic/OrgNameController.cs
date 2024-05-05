using System.Data;
using Microsoft.AspNetCore.Mvc;
using Accountool.Services;
using Accountool.Models;

namespace Accountool.Controllers;
[ApiController]
public partial class OrgNameController : Controller
{
    private readonly IOrgNameService _service;
    public OrgNameController(IOrgNameService service)
    {
        _service = service;
    }

    [HttpGet("OrgNames", Name = "GetOrgNames")]
    [ProducesResponseType(typeof(IEnumerable<OrgName>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("OrgNames/{id:int}", Name = "GetOrgNameById")]
    [ProducesResponseType(typeof(OrgName), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetById(id));
    }

    [HttpPost("OrgNames", Name = "CreateOrgName")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Post([FromBody] OrgName entity)
    {
        return Ok(await _service.Create(entity));
    }

    [HttpPut("OrgNames", Name = "UpdateOrgName")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Put([FromBody] OrgName entity)
    {
        await _service.Update(entity);
        return Ok();
    }

    [HttpDelete("OrgNames/{id:int}", Name = "DeleteOrgName")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }
}