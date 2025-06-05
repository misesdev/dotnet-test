using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.Service;
using api.Models;

namespace api.Controllers;

[ApiController]
[Route("movies")]
[Authorize]
public class MovieController : ControllerBase 
{
    private readonly MovieService _service;
    public MovieController(MovieService service) 
    {
        this._service = service;
    }

    [HttpGet("movie/{id:guid}")]
    public async Task<IResult> Get(Guid id)
    {
        var response = await _service.GetById(id);

        if(!response.Success)
            return Results.BadRequest(response.Message);

        return Results.Ok(response.Data);
    }

    [HttpPost("new")]
    public async Task<IResult> Add(RecordMovie model)
    {
        var response = await _service.AddAsync(model);

        if(!response.Success)
            return Results.BadRequest(response.Message);

        return Results.Ok(response.Data);
    }

    [HttpPost("search")]
    public async Task<IResult> Search(MovieSearch model)
    {
        var response = await _service.SearchByTitle(model);

        if(!response.Success)
            return Results.BadRequest(response.Message);

        return Results.Ok(response.Data);
    }
}
