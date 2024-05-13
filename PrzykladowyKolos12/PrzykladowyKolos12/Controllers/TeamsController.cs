using Microsoft.AspNetCore.Mvc;
using PrzykladowyKolos.ENUM_s;
using PrzykladowyKolos12.DTOs;
using PrzykladowyKolos12.Services;

namespace PrzykladowyKolos12.Controllers;

[Route("api/teams")]
[ApiController]
public class TeamsController : ControllerBase
{
    private ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTeamsAsync(int id)
    {
        var book = await _teamService.GetTeamsAsync(id);
        if (book == null)
            return StatusCode(StatusCodes.Status400BadRequest);
        return Ok(book);
    }
    
    [HttpPost("/api/player")]
    public async Task<IActionResult> AddPlayerAsync(int idPlayer, int idTeam)
    {
        var error = await _teamService.AddPlayerAsync(idPlayer, idTeam);
        if (error == Errors.NotFound)
            return StatusCode(StatusCodes.Status404NotFound);
        if (error == Errors.TooOld)
            return StatusCode(StatusCodes.Status400BadRequest);
        
        return Ok();
    }
}