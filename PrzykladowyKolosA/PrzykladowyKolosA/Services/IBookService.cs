using PrzykladowyKolosA.DTOs;

namespace PrzykladowyKolosA.Services;

public interface IBookService
{
    Task<ReturnBookDTO> GetGenresAsync(int id);
    Task<ReturnBookDTO> AddBookAsync(NewBookDTO newBookDTO);
}