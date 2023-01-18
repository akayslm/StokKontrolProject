using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Repositories.Context;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<Category> _service;

        public CategoryController(IGenericService<Category> service)
        {
            _service = service;
        }

        // GET: api/Category
        [HttpGet]
        public IActionResult TumKategorileriGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/Category/AktifKategorileriGetir
        [HttpGet()]
        public IActionResult AktifKategorileriGetir()
        {
            return Ok(_service.GetActive());
        }

        // GET: api/Category/IdyeGoreKategoriGetir/5
        [HttpGet("{id}")]
        public IActionResult IdyeGoreKategoriGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult KategoriEkle(Category category)
        {
            _service.Add(category);

            return CreatedAtAction("IdyeGoreKategoriGetir", new { id = category.ID }, category);
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult KategoriGuncelle(int id, Category category)
        {
            if (id != category.ID)
            {
                return BadRequest();
            }

            //_service.Entry(category).State = EntityState.Modified;

            try
            {
                _service.Update(category);
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }                
            }
            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public IActionResult KategoriSil(int id)
        {
            var category = _service.GetByID(id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _service.Remove(category);
                return Ok("Kategori Silindi!");
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok("Kategori Silindi!");
        }

        private bool CategoryExists(int id)
        {
            return _service.Any(e => e.ID == id);
        }

        [HttpGet("{id}")]
        public IActionResult KategoriAktiflestir(int id)
        {
            var category = _service.GetByID(id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _service.Activate(id);
                return Ok(_service.GetByID(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }
    }
}
