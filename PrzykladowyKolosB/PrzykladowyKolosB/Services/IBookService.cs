using PrzykladowyKolosB.DTOs;

namespace PrzykladowyKolosB.Services;

public interface IBookService
{
    Task<ReturnBookDTO> GetGenresAsync(int id);
    Task<ReturnBookDTO> AddBookAsync(NewBookDTO newBookDTO);
}