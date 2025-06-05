using Microsoft.AspNetCore.Mvc;
using api.Service;
using api.Models;

namespace api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase 
{
    private readonly AuthService _service;
    public AuthController(AuthService service) 
    {
        this._service = service;
    }

    [HttpPost("signup")]
    public async Task<IResult> SignUp(RecordUser user)
    {
        var response = await _service.SignUp(user);

        if(!response.Success) 
            return Results.BadRequest(new { message = response.Message });

        return Results.Ok(response);
    } 

    [HttpPost("sign")]
    public async Task<IResult> Sign(SignUser model)
    {
        var response = await _service.Sigin(model);

        if(!response.Success)
            Results.BadRequest();

        return Results.Ok(response.Data);
    }
}
