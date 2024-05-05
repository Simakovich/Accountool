using System.Data;
using Microsoft.AspNetCore.Mvc;
using Accountool.Services;
using Accountool.Models;

namespace Accountool.Controllers;
[ApiController]
public partial class OrganizationController : Controller
{
    private readonly IOrganizationService _service;
    public OrganizationController(IOrganizationService service)
    {
        _service = service;
    }

    [HttpGet("Organizations", Name = "GetOrganizations")]
    [ProducesResponseType(typeof(IEnumerable<Organization>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("Organizations/{id:int}", Name = "GetOrganizationById")]
    [ProducesResponseType(typeof(Organization), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetById(id));
    }

    [HttpPost("Organizations", Name = "CreateOrganization")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Post([FromBody] Organization entity)
    {
        return Ok(await _service.Create(entity));
    }

    [HttpPut("Organizations", Name = "UpdateOrganization")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Put([FromBody] Organization entity)
    {
        await _service.Update(entity);
        return Ok();
    }

    [HttpDelete("Organizations/{id:int}", Name = "DeleteOrganization")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }

    [HttpGet("OrgNames/{OrgNameId}/Organizations", Name = "GetOrganizationsForOrgName")]
    [ProducesResponseType(typeof(IEnumerable<Organization>), 200)]
    public async Task<IActionResult> GetOrganizationsForOrgName(int OrgNameId)
    {
        return Ok(await _service.GetForOrgName(OrgNameId));
    }
}