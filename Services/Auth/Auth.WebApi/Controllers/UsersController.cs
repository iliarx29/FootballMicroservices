using Auth.Application.Users.Commands;
using Auth.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _sender.Send(new GetUsersQuery());

        if (!result.IsSuccess)
            return NotFound();

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUsers(Guid id)
    {
        var result = await _sender.Send(new GetUserByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
    {
        var result = await _sender.Send(command);

        if (!result.IsSuccess)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _sender.Send(new DeleteUserCommand(id));

        if (!result.IsSuccess)
            return NotFound();

        return NoContent();
    }
}
