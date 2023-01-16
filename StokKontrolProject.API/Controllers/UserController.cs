using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> _service;

        public UserController(IGenericService<User> service)
        {
            _service = service;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult TumKullanıcılarıGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/User/AktifKategorileriGetir
        [HttpGet()]
        public IActionResult AktifKullanıcılarıGetir()
        {
            return Ok(_service.GetActive());
        }

        // GET: api/User/IdyeGoreKategoriGetir/5
        [HttpGet("{id}")]
        public IActionResult IdyeGoreKullanıcıGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        [HttpGet]
        public IActionResult Login(string email, string parola)
        {
            if (_service.Any(x=>x.Email == email && x.Password == parola))
            {
                User logged = _service.GetByDefault(x=>x.Email == email && x.Password == parola);
                return Ok(logged);
            }
            return NotFound();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult KullanıcıEkle(User user)
        {
            _service.Add(user);

            return CreatedAtAction("IdyeGoreKullanıcıGetir", new { id = user.ID }, user);
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult KullanıcıGuncelle(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }

            //_service.Entry(User).State = EntityState.Modified;

            try
            {
                _service.Update(user);
                return Ok(User);
            }
            catch (Exception) //incele
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
            }
            return NoContent();
        }


        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public IActionResult KullanıcıSil(int id)
        {
            var user = _service.GetByID(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _service.Remove(user);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok("Kullanıcı Silindi!");
        }

        private bool UserExists(int id)
        {
            return _service.Any(e => e.ID == id);
        }

        [HttpGet()]
        public IActionResult KullanıcılarıAktiflestir(int id)
        {
            var User = _service.GetByID(id);
            if (User == null)
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
