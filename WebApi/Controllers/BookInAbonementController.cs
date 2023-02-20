using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Service;
using WebApi.Validators;

namespace WebApi.Controllers;
[Route("api/[controller]")]
[Authorize]
public class BookAbonementController : Controller
{
    
        private readonly BookInAbonementService _bookInService;
        private readonly IValidator<BookInAbonementCreateDto> _validator;
        private readonly BookInStockValidator _bookInStockValidator;
        private readonly BookInAbonementValidator _bookInAbonementValidator;

        public BookAbonementController(BookInAbonementService bookInService, IValidator<BookInAbonementCreateDto> validator, BookInAbonementValidator bookInAbonementValidator, BookInStockValidator bookInStockValidator)
        {
            _bookInService = bookInService;
            _validator = validator;
            _bookInAbonementValidator = bookInAbonementValidator;
            _bookInStockValidator = bookInStockValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookInAbonementDto>>> GetAll()
        {
            var books = await _bookInService.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookInAbonementDto>> GetById(int id)
        {
            var book = await _bookInService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Abonate(BookInAbonementCreateDto book)
        {
            var validationResult = await _validator.ValidateAsync(book);
           await _bookInStockValidator.ValidateBookInStockAmount(book.BookId);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            
            await _bookInService.AbonateBook(book);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BookInAbonementDto bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = await _bookInService.Update(bookDto, id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

    

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var canDelete = await _bookInAbonementValidator.DeleteIsValid(id);
            if (!canDelete)
            {
                return BadRequest("BookInAbonement is in use and cannot be deleted");
            }
            await _bookInService.Delete(id);
            return Ok();
        }

        [HttpPost("anything")] 
        public async Task<IActionResult> GiveBookBack(int id,int bookid)
        {
            await _bookInService.GiveBookBack(id,bookid);
            return Ok();
        }
    }

