using Microsoft.AspNetCore.Mvc;
using PrzykladowyKolosB.DTOs;
using PrzykladowyKolosB.Services;

namespace PrzykladowyKolosB.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    private IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{id:int}/authors")]
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