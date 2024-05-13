using PrzykladowyKolos.ENUM_s;
using PrzykladowyKolos12.DTOs;

namespace PrzykladowyKolos12.Services;

public interface ITeamService
{
    Task<List<TeamDTO>> GetTeamsAsync(int id);
    Task<Errors> AddPlayerAsync(int idPlayer, int idTeam);
}