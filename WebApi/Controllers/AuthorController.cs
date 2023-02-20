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
public class AuthorController : Controller
{
    private readonly AuthorService _authorService;
    private readonly IValidator<AuthorCreateDto> _validator;
    private readonly AuthorValidator _authorValidator;
    public AuthorController(AuthorService authorService, IValidator<AuthorCreateDto> validator, AuthorValidator authorValidator)
    {
        _authorService = authorService;
        _validator = validator;
        _authorValidator = authorValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var authors = await _authorService.GetAll();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var author = await _authorService.GetById(id);
        if (author == null)
            return NotFound();

        return Ok(author);
    }

    [HttpPost]
    public  async Task<IActionResult> Insert(AuthorCreateDto authorCreateDto)
    {
        var validationResult = await _validator.ValidateAsync(authorCreateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

       _authorService.Insert(authorCreateDto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AuthorDto authorDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var author = await _authorService.Update(authorDto, id);
        if (author == null)
            return NotFound();

        return Ok(author);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var canDelete = await _authorValidator.DeleteIsValid(id);
        if (!canDelete)
        {
            return BadRequest("Abonement is in use and cannot be deleted");
        }
        await _authorService.Delete(id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll()
    {
        await _authorService.DeleteAll();
        return Ok();
    }
}

    
