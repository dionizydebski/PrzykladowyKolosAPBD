using PrzykladowyKolosA.DTOs;

namespace PrzykladowyKolosA.Repositories;

public interface IBookRepository
{
    Task<ReturnBookDTO> GetGenresAsync(int id);
    Task<ReturnBookDTO> AddBookAsync(NewBookDTO newBookDTO);
}