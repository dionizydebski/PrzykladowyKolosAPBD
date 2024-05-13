using PrzykladowyKolos.ENUM_s;
using PrzykladowyKolos12.DTOs;
using PrzykladowyKolos12.Repositories;

namespace PrzykladowyKolos12.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;

    public TeamService(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }
    public async Task<List<TeamDTO>> GetTeamsAsync(int id)
    {
        return await _teamRepository.GetTeamsAsync(id);
    }

    public async Task<Errors> AddPlayerAsync(int idPlayer, int idTeam)
    {
        var age = await _teamRepository.GetAgeAsync(idPlayer);
        
        var maxAge = await _teamRepository.GetMaxAgeFromTeamAsync(idTeam);
        
        if (age == 0 || maxAge == 0)
            return Errors.NotFound;
        
        if (age > maxAge)
            return Errors.TooOld;
        
        await _teamRepository.AddPlayerAsync(idPlayer, idTeam);
        return Errors.Good;
    }
}