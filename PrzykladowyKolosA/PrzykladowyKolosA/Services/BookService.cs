using PrzykladowyKolosA.DTOs;
using PrzykladowyKolosA.Repositories;

namespace PrzykladowyKolosA.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task<ReturnBookDTO> GetGenresAsync(int id)
    {
        return await _bookRepository.GetGenresAsync(id);
    }

    public async Task<ReturnBookDTO> AddBookAsync(NewBookDTO newBookDTO)
    {
        return await _bookRepository.AddBookAsync(newBookDTO);
    }
}