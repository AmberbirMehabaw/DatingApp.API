using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class ValuesController : ControllerBase
{
        private readonly DataContext _context;
   public ValuesController(DataContext context)
   {
            _context = context;
    
   }
    [AllowAnonymous]
    [HttpGet]
    public async  Task<IActionResult> GetValues()
    {
        var values = await _context.Values.ToListAsync();
        return Ok(values);
    }
    [AllowAnonymous]
     [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
       var value  = await _context.Values.FirstOrDefaultAsync(x=> x.Id == id);
        return Ok(value);
    }

    // [HttpPost] 
    // public async Task<IActionResult> create(Value value){
    //     var value = await _context.Values.AddAsync( = value.Id, Name = value.Name);
    // }
}
