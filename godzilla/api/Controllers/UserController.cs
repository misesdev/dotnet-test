using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.Service;

namespace api.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UserController : ControllerBase 
{
    private readonly UserService _service;
    
    public UserController(UserService service) 
    {
        this._service = service;
    }

    [HttpGet("user/{id:guid}")]
    public async Task<IResult> Get(Guid id) 
    {
        var result = await _service.GetUser(id); 
        
        if(!result.Success)
            return Results.NotFound(result.Message);

        return Results.Ok(result.Data);
    }
}
