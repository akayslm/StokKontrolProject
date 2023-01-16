using Microsoft.AspNetCore.Mvc;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> _service;

        public SupplierController(IGenericService<Supplier> service)
        {
            _service = service;
        }

        // GET: api/Supplier
        [HttpGet]
        public IActionResult TumTedarikcileriGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/Supplier/AktifKategorileriGetir
        [HttpGet()]
        public IActionResult AktifTedarikcileriGetir()
        {
            return Ok(_service.GetActive());
        }

        // GET: api/Supplier/IdyeGoreKategoriGetir/5
        [HttpGet("{id}")]
        public IActionResult IdyeGoreTedarikciGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        // POST: api/Supplier
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult TedarikciEkle(Supplier supplier)
        {
            _service.Add(supplier);

            return CreatedAtAction("IdyeGoreTedarikciGetir", new { id = supplier.ID }, supplier);
        }

        // PUT: api/Supplier/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult TedarikciGuncelle(int id, Supplier supplier)
        {
            if (id != supplier.ID)
            {
                return BadRequest();
            }

            //_service.Entry(Supplier).State = EntityState.Modified;

            try
            {
                _service.Update(supplier);
                return Ok(supplier);
            }
            catch (Exception) //incele
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
            }
            return NoContent();
        }

        // DELETE: api/Supplier/5
        [HttpDelete("{id}")]
        public IActionResult TedarikciSil(int id)
        {
            var supplier = _service.GetByID(id);
            if (supplier == null)
            {
                return NotFound();
            }

            try
            {
                _service.Remove(supplier);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok("Tedarikçi Silindi!");
        }

        private bool SupplierExists(int id)
        {
            return _service.Any(e => e.ID == id);
        }

        [HttpGet()]
        public IActionResult TedarikcileriAktiflestir(int id)
        {
            var supplier = _service.GetByID(id);
            if (supplier == null)
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

