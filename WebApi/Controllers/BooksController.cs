using ClassLibrary1;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Service;
using WebApi.Validators;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BooksController : Controller
    {
        private readonly BookService _bookService;
        private readonly BookValidator _bookValidator;
        private readonly IValidator<BookCreateDto> _validator;

        public BooksController(BookService bookService, BookValidator bookValidator, IValidator<BookCreateDto> validator)
        {
            _bookService = bookService;
            _bookValidator = bookValidator;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await _bookService.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] BookCreateDto book)
        {
            var validationResult = await _validator.ValidateAsync(book);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            
            await _bookService.Insert(book);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BookDto bookDto)
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
            var canDelete = await _bookValidator.DeleteIsValid(id);
            if (!canDelete)
            {
                return BadRequest("Book is in use and cannot be deleted");
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
}