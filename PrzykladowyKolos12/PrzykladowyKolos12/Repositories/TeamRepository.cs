using System.Data.SqlClient;
using PrzykladowyKolos12.DTOs;

namespace PrzykladowyKolos12.Repositories;

public class TeamRepository : ITeamRepository
{
    private IConfiguration _configuration;

    public TeamRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<List<TeamDTO>> GetTeamsAsync(int id)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        
        await using var cmd = new SqlCommand("SELECT t.idTeam, TeamName, MaxAge, Score FROM Team t JOIN Championship_Team ct ON t.IdTeam = ct.IdTeam JOIN Championship c ON ct.IdChampionship = c.IdChampionship WHERE c.IdChampionship = @id",con);
        cmd.Parameters.AddWithValue("@id", id);
        
        var dr = await cmd.ExecuteReaderAsync();

        var teams = new List<TeamDTO>();
        
        if (dr.HasRows)
        { 
            while (await dr.ReadAsync())
            {
                teams.Add(new TeamDTO(idTeam: (int)dr["idTeam"],name: (string)dr["TeamName"], maxAge: (int)dr["MaxAge"], score: (double)dr["Score"]));
            }
            await dr.CloseAsync();
        }
        else
            return null;

        await con.CloseAsync();
        return teams;
    }

    public async Task<int> GetAgeAsync(int idPlayer)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        
        await using var cmdAge = new SqlCommand("SELECT DateOfBirth FROM Player WHERE IdPlayer = @idPlayer",con);
        cmdAge.Parameters.AddWithValue("@idPlayer", idPlayer);
        
        var drAge = await cmdAge.ExecuteReaderAsync();

        DateTime dateOfBirth;
        
        if (drAge.HasRows)
        { 
            await drAge.ReadAsync();
        
            dateOfBirth = (DateTime)drAge["DateOfBirth"];
        
            await drAge.CloseAsync();
        }
        else
            return 0;

        int age = (int)(DateTime.Now.Subtract(dateOfBirth).TotalDays / 365.25);

        await con.CloseAsync();
        return age;
    }

    public async Task<int> GetMaxAgeFromTeamAsync(int idTeam)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        
        await using var cmdMaxAge = new SqlCommand("SELECT MaxAge FROM Team WHERE IdTeam = @idTeam",con);
        cmdMaxAge.Parameters.AddWithValue("@idTeam", idTeam);
        
        var drMaxAge = await cmdMaxAge.ExecuteReaderAsync();

        int maxAge;
        
        if (drMaxAge.HasRows)
        { 
            await drMaxAge.ReadAsync();
        
            maxAge = (int)drMaxAge["MaxAge"];
        
            await drMaxAge.CloseAsync();
        }
        else
            return 0;

        await con.CloseAsync();
        return maxAge;
    }

    public async Task<int> AddPlayerAsync(int idPlayer, int idTeam)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        
        await using var cmdTeam = new SqlCommand("INSERT INTO Player_Team (IdPlayer, IdTeam, NumOnShirt) VALUES(@idPlayer, @idTeam, 11)",con);
        cmdTeam.Parameters.AddWithValue("@idPlayer", idPlayer);
        cmdTeam.Parameters.AddWithValue("@idTeam", idTeam);
        var raTeam = await cmdTeam.ExecuteNonQueryAsync();
        if (raTeam == 0)
            return 0;
         
        await con.CloseAsync();
        return raTeam;
    }
}