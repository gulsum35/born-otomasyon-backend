using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BornOtomasyonApi.Data;
using BornOtomasyonApi.Models;

namespace BornOtomasyonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Bu controller içindeki tüm endpoint'ler token ile korunur
    public class FormDataController : ControllerBase
    {
        private readonly BornOtomasyonContext _context;

        public FormDataController(BornOtomasyonContext context)
        {
            _context = context;
        }

        // GET: 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormData>>> GetAll()
        {
            var list = await _context.FormDatas.ToListAsync();
            return Ok(list);
        }

        // GET: 
        [HttpGet("{id}")]
        public async Task<ActionResult<FormData>> GetById(int id)
        {
            var item = await _context.FormDatas.FindAsync(id);

            if (item == null) return NotFound();

            return Ok(item);
        }

        // POST: yeni kayıt ekleriz 
        [HttpPost]
        public async Task<ActionResult<FormData>> Create(FormData formData)
        {
            _context.FormDatas.Add(formData);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = formData.Id }, formData);
        }

        // PUT: update işlemin yapar
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FormData formData)
        {
            if (id != formData.Id) return BadRequest();

            var existing = await _context.FormDatas.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Text1 = formData.Text1;
            existing.Num1 = formData.Num1;
            existing.Date1 = formData.Date1;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.FormDatas.FindAsync(id);
            if (item == null) return NotFound();

            _context.FormDatas.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
