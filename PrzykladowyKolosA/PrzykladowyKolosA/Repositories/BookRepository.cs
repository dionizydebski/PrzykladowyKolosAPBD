using System.Data.SqlClient;
using PrzykladowyKolosA.DTOs;

namespace PrzykladowyKolosA.Repositories;

public class BookRepository : IBookRepository
{
    private IConfiguration _configuration;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<ReturnBookDTO> GetGenresAsync(int id)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        
        //Znajdujemy ksiazke
        
        await using var cmd_id_title = new SqlCommand();
        cmd_id_title.Connection = con;
        
        cmd_id_title.Parameters.AddWithValue("@id", id);
        cmd_id_title.CommandText = "SELECT PK, title FROM books WHERE PK = @id";
        
        var dr = await cmd_id_title.ExecuteReaderAsync();

        int idBook;
        string title;

        if (dr.HasRows)
        { 
            await dr.ReadAsync();
            
            idBook = (int)dr["PK"];
            title = (string)dr["title"];
            
            await dr.CloseAsync();
        }
        else
            return null;
        
        //Znajdujemy ksiazki
        
        await using var cmd_genres = new SqlCommand();
        cmd_genres.Connection = con;
        
        cmd_genres.Parameters.AddWithValue("@id", id);
        cmd_genres.CommandText = "SELECT g.name FROM books b JOIN books_genres bg ON b.PK = bg.FK_book JOIN genres g ON bg.FK_genre = g.PK WHERE b.PK = @id";
        
        var dr_genres = await cmd_genres.ExecuteReaderAsync();

        var genres = new List<string>();
        
        if (dr_genres.HasRows)
        { 
            while (await dr_genres.ReadAsync())
            {
                genres.Add((string)dr_genres["name"]);
            }
            await dr_genres.CloseAsync();
        }
        else
            return null;
        
        var book = new ReturnBookDTO
        (
            idBook: idBook,
            title: title,
            genres: genres
        );
        return book;
    }

    public async Task<ReturnBookDTO> AddBookAsync(NewBookDTO newBookDTO)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        
        //Dodajmy ksiazke
        
        await using var cmdAddBook = new SqlCommand("INSERT INTO books(title) VALUES(@title); SELECT SCOPE_IDENTITY() as PK;",con);
        cmdAddBook.Parameters.AddWithValue("@title", newBookDTO.title);
        
        var dr = await cmdAddBook.ExecuteReaderAsync();
        
        await dr.ReadAsync();
        
        var pk = Convert.ToInt32(dr["PK"]);

        await dr.CloseAsync();
        
        var rowsAffected = await cmdAddBook.ExecuteNonQueryAsync();
        if (rowsAffected == 0)
            return null;
        
        //Dodajemy gatunki
        
        foreach (var idGenre in newBookDTO.genres)
        {
            await using var tmp = new SqlCommand("INSERT INTO books_genres (FK_book, FK_genre) VALUES(@idBook, @idGenre)",con);
            tmp.Parameters.AddWithValue("@idGenre", idGenre);
            tmp.Parameters.AddWithValue("@idBook", pk);
            var ra = await tmp.ExecuteNonQueryAsync();
            if (ra == 0)
                return null;
        }
        
        //Znajdujemy gatunki
        
        await using var cmdGenres = new SqlCommand("SELECT g.name FROM books b JOIN books_genres bg ON b.PK = bg.FK_book JOIN genres g ON bg.FK_genre = g.PK WHERE b.PK = @id",con);
        cmdGenres.Parameters.AddWithValue("@id", pk);
        
        var drGenres = await cmdGenres.ExecuteReaderAsync();

        var genres = new List<string>();
        
        if (drGenres.HasRows)
        { 
            while (await drGenres.ReadAsync())
            {
                genres.Add((string)drGenres["name"]);
            }
            await drGenres.CloseAsync();
        }
        else
            return null;
        
        var result = new ReturnBookDTO
        (
            idBook: pk,
            title: newBookDTO.title,
            genres: genres
        );
        
        return result;
    }
}