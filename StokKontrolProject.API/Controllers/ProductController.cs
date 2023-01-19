using Microsoft.AspNetCore.Mvc;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> _service;

        public ProductController(IGenericService<Product> service)
        {
            _service = service;
        }

        // GET: api/Product
        [HttpGet]
        public IActionResult TumUrunleriGetir()
        {
            return Ok(_service.GetAll(t0=>t0.Kategori, t1=>t1.Tedarikci));
        }

        // GET: api/Product/AktifÜrünleriGetir
        [HttpGet()]
        public IActionResult AktifUrunleriGetir()
        {
            return Ok(_service.GetActive(t0 => t0.Kategori, t1 => t1.Tedarikci));
        }

        // GET: api/Product/IdyeGoreÜrünGetir/5
        [HttpGet("{id}")]
        public IActionResult IdyeGoreUrunGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult UrunEkle(Product product)
        {
            _service.Add(product);

            return CreatedAtAction("IdyeGoreUrunGetir", new { id = product.ID }, product);
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult UrunGuncelle(int id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }

            //_service.Entry(Product).State = EntityState.Modified;

            try
            {
                _service.Update(product);
                return Ok(product);
            }
            catch (Exception) //incele
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
            }
            return NoContent();
        }


    // DELETE: api/Product/5
    [HttpDelete("{id}")]
        public IActionResult UrunSil(int id)
        {
            var Product = _service.GetByID(id);
            if (Product == null)
            {
                return NotFound();
            }

            try
            {
                _service.Remove(Product);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok("Ürün Silindi!");
        }

        private bool ProductExists(int id)
        {
            return _service.Any(e => e.ID == id);
        }

        [HttpGet()]
        public IActionResult UrunleriAktiflestir(int id)
        {
            var Product = _service.GetByID(id);
            if (Product == null)
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
