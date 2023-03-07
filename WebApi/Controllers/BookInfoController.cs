using ClassLibrary1;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApi.Service;
using WebApi.Validators;

namespace WebApi.Controllers;


    [Route("api/[controller]")]
    [Authorize]
    public class BookInfoController : Controller
    {
        private readonly BookInfoService _bookService;
        private readonly IValidator<BookInfoCreateDto> _validator;
        private readonly BookInfoValidator _bookInfoValidator;

        public BookInfoController(BookInfoService bookService, IValidator<BookInfoCreateDto> validator, BookInfoValidator bookInfoValidator)
        {
            _bookService = bookService;
            _validator = validator;
            _bookInfoValidator = bookInfoValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookInfoDto>>> GetAll()
        {
            var books = await _bookService.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookInfoDto>> GetById(int id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(BookInfoCreateDto book)
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
        public async Task<IActionResult> Update(int id, BookInfoDto bookDto)
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
            var canDelete = await _bookInfoValidator.DeleteIsValid(id);
            if (!canDelete)
            {
                return BadRequest("BookInfo is in use and cannot be deleted");
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
