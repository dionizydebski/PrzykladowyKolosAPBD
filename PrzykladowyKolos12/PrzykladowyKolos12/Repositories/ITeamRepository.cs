using PrzykladowyKolos12.DTOs;

namespace PrzykladowyKolos12.Repositories;

public interface ITeamRepository
{
    Task<List<TeamDTO>> GetTeamsAsync(int id);
    Task<int> GetAgeAsync(int idPlayer);
    Task<int> GetMaxAgeFromTeamAsync(int idTeam);
    Task<int> AddPlayerAsync(int idPlayer, int idTeam);
}