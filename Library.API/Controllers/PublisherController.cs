using ClassLibrary1;
using FluentValidation;
using Library.Core.Service;
using Library.Core.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class PublisherController : Controller
{
    private readonly PublisherService _publisherService;
    private readonly IValidator<PublisherCreateDto> _validator;
    private readonly PublisherValidator _publisherValidator;

    public PublisherController(PublisherService publisherService, IValidator<PublisherCreateDto> validator, PublisherValidator publisherValidator)
    {
        _publisherService = publisherService;
        _validator = validator;
        _publisherValidator = publisherValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublisherDto>>> GetAll()
    {
        var books = await _publisherService.GetAll();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PublisherDto>> GetById(int id)
    {
        var book = await _publisherService.GetById(id);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] PublisherCreateDto publisher)
    {
        var validationResult = await _validator.ValidateAsync(publisher);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        _publisherService.Insert(publisher);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PublisherDto bookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var publisher = await _publisherService.Update(bookDto, id);
        if (publisher == null)
            return NotFound();

        return Ok(publisher);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var canDelete = await _publisherValidator.DeleteIsValid(id);
        if (!canDelete)
        {
            return BadRequest("Publisher is in use and cannot be deleted");
        }
        await _publisherService.Delete(id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll()
    {
        await _publisherService.DeleteAll();
        return Ok();
    }
}