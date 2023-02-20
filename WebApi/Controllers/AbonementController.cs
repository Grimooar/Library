using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Service;
using WebApi.Validators;


namespace WebApi.Controllers;
[Route("api/[controller]")]
[Authorize]
public class AbonementController : Controller
{
    private readonly AbonementService _abonementService;
    private readonly IValidator<AbonementCreateDto> _validator;
    private readonly AbonementValidator _abonementValidator;


    public AbonementController(AbonementService abonementService, IValidator<AbonementCreateDto> validator, AbonementValidator abonementValidator)
    {
        _abonementService = abonementService;
        _validator = validator;
        _abonementValidator = abonementValidator;
    }
    [HttpGet]
   
    public async Task<IActionResult> GetAll()
    {
        var abonement = await _abonementService.GetAll();
        return Ok(abonement);
    }

    [HttpGet("{id}")]
    
    public async Task<IActionResult> GetById(int id)
    {
        var abonement = await _abonementService.GetById(id);
        if (abonement == null)
            return NotFound();

        return Ok(abonement);
    }

    [HttpPost]
  
    public async Task<IActionResult> Insert(AbonementCreateDto abonementCreateDto)
    {
       
        var validationResult = await _validator.ValidateAsync(abonementCreateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        _abonementService.Insert(abonementCreateDto);
        return Ok();
    }

    [HttpPut("{id}")]
   
    public async Task<IActionResult> Update(int id, AbonementDto abonementDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var author = await _abonementService.Update(abonementDto, id);
        if (author == null)
            return NotFound();

        return Ok(author);
    }

    
    
    [HttpDelete("{id}")]
    
    public async Task<IActionResult> Delete(int id)
    {
        var canDelete = await _abonementValidator.DeleteIsValid(id);
        if (!canDelete)
        {
            return BadRequest("Abonement is in use and cannot be deleted");
        }

        await _abonementService.Delete(id);
        return Ok();
    }
    [HttpDelete]
    
    public async Task<IActionResult> DeleteAll()
    {
        await _abonementService.DeleteAll();
        return Ok();
    }
}