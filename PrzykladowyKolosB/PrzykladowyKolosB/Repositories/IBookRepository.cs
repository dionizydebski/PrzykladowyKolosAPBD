using PrzykladowyKolosB.DTOs;

namespace PrzykladowyKolosB.Repositories;

public interface IBookRepository
{
    Task<ReturnBookDTO> GetGenresAsync(int id);
    Task<ReturnBookDTO> AddBookAsync(NewBookDTO newBookDTO);
}