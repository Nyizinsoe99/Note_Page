// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.OData.Query;
// using Microsoft.AspNetCore.OData.Routing.Controllers;
// using Microsoft.EntityFrameworkCore;
// using Node_Page.Model;

// namespace Node_Page.Controller;

// public class NotesController : ODataController
// {
//     private readonly AppDbContext _context;

//     public NotesController(AppDbContext context)
//     {
//         _context = context;
//     }

//     [EnableQuery]
//     [HttpGet]
//     public IQueryable<Note> Get()
//     {
//         return _context.Notes.AsNoTracking();
//     }

//     [EnableQuery]
//     [HttpPost]
//     public async Task<IActionResult> Post([FromBody] Note note)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);

//         _context.Notes.Add(note);
//         await _context.SaveChangesAsync();
//         return Created(note);
//     }
// }

//---------------------------------------------------------------------------

// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.OData.Query;
// using Microsoft.AspNetCore.OData.Routing.Controllers;
// using Microsoft.AspNetCore.OData.Formatter; // Required for [FromODataUri]
// using Node_Page.Model;

// namespace Node_Page.Controller;

// public class NotesController : ODataController
// {
//     private readonly AppDbContext _context;
//     public NotesController(AppDbContext context) => _context = context;

//     [EnableQuery]
//     public IQueryable<Note> Get() => _context.Notes;

//     [EnableQuery]
//     public async Task<IActionResult> Post([FromBody] Note note)
//     {
//         if (!ModelState.IsValid) return BadRequest(ModelState);
//         _context.Notes.Add(note);
//         await _context.SaveChangesAsync();
//         return Created(note);
//     }

//     // --- NEW DELETE METHOD ---
//     [HttpDelete]
//     public async Task<IActionResult> Delete([FromODataUri] int key)
//     {
//         var note = await _context.Notes.FindAsync(key);
//         if (note == null) return NotFound();

//         _context.Notes.Remove(note);
//         await _context.SaveChangesAsync();
//         return NoContent();
//     }
// }

//-------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Formatter;
using Node_Page.Model;

namespace Node_Page.Controller;

public class NotesController : ODataController
{
    private readonly AppDbContext _context;
    public NotesController(AppDbContext context) => _context = context;

    [EnableQuery]
    public IQueryable<Note> Get() => _context.Notes;

    [EnableQuery]
    public async Task<IActionResult> Post([FromBody] Note note)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
        return Created(note);
    }

    // This handles the DELETE request
    [HttpDelete]
    public async Task<IActionResult> Delete([FromODataUri] int key)
    {
        var note = await _context.Notes.FindAsync(key);
        if (note == null) return NotFound();

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}