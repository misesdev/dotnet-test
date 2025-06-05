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
        var response = await _service.GetUser(id); 
        
        if(!response.Success)
            return Results.NotFound(response.Message);

        return Results.Ok(response.Data);
    }
}
