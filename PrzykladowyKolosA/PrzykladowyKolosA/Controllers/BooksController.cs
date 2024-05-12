using Microsoft.AspNetCore.Mvc;
using PrzykladowyKolosA.DTOs;
using PrzykladowyKolosA.Services;

namespace PrzykladowyKolosA.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    private IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{id:int}/genres")]
    public async Task<IActionResult> GetGenresAsync(int id)
    {
        var book = await _bookService.GetGenresAsync(id);
        if (book == null)
            return StatusCode(StatusCodes.Status400BadRequest);
        return Ok(book);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBookAsync(NewBookDTO newBookDTO)
    {
        var book = await _bookService.AddBookAsync(newBookDTO);
        if (book == null)
            return StatusCode(StatusCodes.Status400BadRequest);
        return Ok(book);
    }
}