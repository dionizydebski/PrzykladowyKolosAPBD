using System.Data.SqlClient;
using PrzykladowyKolosB.DTOs;

namespace PrzykladowyKolosB.Repositories;

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
        
        await using var cmd_id_title = new SqlCommand("SELECT PK, title FROM books WHERE PK = @id",con);
        cmd_id_title.Parameters.AddWithValue("@id", id);
        
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
        
        //Znajdujemy autorow
        
        await using var cmd_genres = new SqlCommand("SELECT first_name, last_name FROM books b JOIN books_authors ba ON b.PK = ba.FK_book JOIN authors a ON a.PK = ba.FK_author WHERE b.PK = @id",con);
        cmd_genres.Parameters.AddWithValue("@id", id);
        
        var dr_authors = await cmd_genres.ExecuteReaderAsync();

        var authors = new List<AutorDTO>();
        
        if (dr_authors.HasRows)
        { 
            while (await dr_authors.ReadAsync())
            {
                authors.Add(new AutorDTO(firstName: (string)dr_authors["first_name"],lastname: (string)dr_authors["last_name"]));
            }
            await dr_authors.CloseAsync();
        }
        else
            return null;
        
        var book = new ReturnBookDTO
        (
            idBook: idBook,
            title: title,
            authors: authors
        );

        await con.CloseAsync();
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
        
        var idBook = Convert.ToInt32(dr["PK"]);

        await dr.CloseAsync();
        
        var rowsAffected = await cmdAddBook.ExecuteNonQueryAsync();
        if (rowsAffected == 0)
            return null;
        
        //Dodajemy autorow
        
        foreach (var authorDTO in newBookDTO.authors)
        {
            await using var cmdIdAutor = new SqlCommand("SELECT PK FROM authors WHERE first_name = @firstName AND last_name = @lastName", con);
            cmdIdAutor.Parameters.AddWithValue("@firstName", authorDTO.firstName);
            cmdIdAutor.Parameters.AddWithValue("@lastName", authorDTO.lastname);

            var drIdAuthor = await cmdIdAutor.ExecuteReaderAsync();

            int idAuthor;

            if (drIdAuthor.HasRows)
            {
                await drIdAuthor.ReadAsync();
                idAuthor = Convert.ToInt32(drIdAuthor["PK"]);
                await drIdAuthor.CloseAsync();
            }
            else
                return null;
            
            await using var tmp = new SqlCommand("INSERT INTO books_authors (FK_book, FK_author) VALUES(@idBook, @idAuthor)",con);
            tmp.Parameters.AddWithValue("@idAuthor", idAuthor);
            tmp.Parameters.AddWithValue("@idBook", idBook);
            var ra = await tmp.ExecuteNonQueryAsync();
            if (ra == 0)
                return null;
        }
        
        var result = new ReturnBookDTO
        (
            idBook: idBook,
            title: newBookDTO.title,
            authors: newBookDTO.authors
        );
        await con.CloseAsync();
        return result;
    }
}