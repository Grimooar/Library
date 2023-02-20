using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Service;
using WebApi.Validators;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class BookInStockController : Controller
{
    private readonly IValidator<BookInStockCreateDto> _validator;
    private readonly BookInStockService _bookService;
    private readonly BookInStockValidator _bookInStockValidator;

    public BookInStockController(BookInStockService bookService, IValidator<BookInStockCreateDto> validator,
        BookInStockValidator bookInStockValidator)
    {
        _bookService = bookService;
        _validator = validator;
        _bookInStockValidator = bookInStockValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookInStockDto>>> GetAll()
    {
        var books = await _bookService.GetAll();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookInStockDto>> GetById(int id)
    {
        var book = await _bookService.GetById(id);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] BookInStockCreateDto book)
    {
        var validationResult = await _validator.ValidateAsync(book);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        _bookService.Insert(book);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BookInStockDto bookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var book = await _bookService.Update(bookDto, id);
        if (book == null)
            return NotFound();

        return Ok(book);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var canDelete = await _bookInStockValidator.DeleteIsValid(id);
        if (!canDelete)
        {
            return BadRequest("BookInStock is in use and cannot be deleted");
        }
        await _bookService.Delete(id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll()
    {
        await _bookService.DeleteAll();
        return Ok();
    }
}