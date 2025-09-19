using Microsoft.AspNetCore.Mvc;
using usecase_01_call_external_api.Abstractions;
using usecase_01_call_external_api.Models;

namespace usecase_01_call_external_api.Controllers;

public class UsersController(IExternalApiClient apiClient) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<User>>> GetUsers(CancellationToken ct)
    {
        var users = await apiClient.GetUsersAsync(ct);
        return Ok(users);
    }
}