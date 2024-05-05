using System.Data;
using Microsoft.AspNetCore.Mvc;
using Accountool.Services;
using Accountool.Models;

namespace Accountool.Controllers;
[ApiController]
public partial class ClientProfileController : Controller
{
    private readonly IClientProfileService _service;
    public ClientProfileController(IClientProfileService service)
    {
        _service = service;
    }

    [HttpGet("ClientProfiles", Name = "GetClientProfiles")]
    [ProducesResponseType(typeof(IEnumerable<ClientProfile>), 200)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("ClientProfiles/{id:int}", Name = "GetClientProfileById")]
    [ProducesResponseType(typeof(ClientProfile), 200)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetById(id));
    }

    [HttpPost("ClientProfiles", Name = "CreateClientProfile")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Post([FromBody] ClientProfile entity)
    {
        return Ok(await _service.Create(entity));
    }

    [HttpPut("ClientProfiles", Name = "UpdateClientProfile")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Put([FromBody] ClientProfile entity)
    {
        await _service.Update(entity);
        return Ok();
    }

    [HttpDelete("ClientProfiles/{id:int}", Name = "DeleteClientProfile")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);
        return Ok();
    }

    [HttpGet("Users/{UserId}/ClientProfiles", Name = "GetClientProfilesForUser")]
    [ProducesResponseType(typeof(IEnumerable<ClientProfile>), 200)]
    public async Task<IActionResult> GetClientProfilesForUser(int UserId)
    {
        return Ok(await _service.GetForUser(UserId));
    }
}