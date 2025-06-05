using Microsoft.AspNetCore.Mvc;
using api.Service;
//using api.Models;

namespace api.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase 
{
    private readonly UserService _service;
    
    public UserController(UserService service) 
    {
        this._service = service;
    }

    // [HttpPost("user")]
    // public async Task<IResult> Register(RecordUser user)
    // {
    //     var response = await _service.Register(user);

    //     if(response.Success) 
    //         return Results.BadRequest(new { message = response.Message });

    //     return Results.Ok(response);
    // }
   
    [HttpPost("{id:guid}")]
    public async Task<IResult> Get(Guid id) 
    {
        var response = await _service.GetUser(id); 
        
        if(!response.Success)
            return Results.NotFound(response.Message);

        return Results.Ok(response.Data);
    }
}
