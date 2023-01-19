using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Entities.Enums;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<User> _userService;
        private readonly IGenericService<OrderDetails> _odService;
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<Product> _productService;

        public OrderController(IGenericService<User> userService, IGenericService<Product> productService, IGenericService<OrderDetails> odService, IGenericService<Order> orderService)
        {
            _userService = userService;
            _odService = odService;
            _orderService = orderService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult TumSiparisleriGetir()
        {
            return Ok(_orderService.GetAll());
        }

        // GET: api/Product/AktifÜrünleriGetir
        [HttpGet()]
        public IActionResult AktifSiparisleriGetir()
        {
            return Ok(_orderService.GetActive());
        }

        // GET: api/Product/IdyeGoreÜrünGetir/5
        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisGetir(int id)
        {
            return Ok(_orderService.GetByID(id, t0=>t0.SiparisDetaylari, t1=>t1.Kullanici));
        }

        // GET: api/Product/IdyeGoreÜrünGetir/5
        [HttpGet]
        public IActionResult BekleyenSiparisleriGetir(int id)
        {
            return Ok(_orderService.GetDefault(x=>x.Status == Status.Pending).ToList());
        }

        [HttpGet]
        public IActionResult OnaylananSiparisleriGetir(int id)
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Confirmed).ToList());
        }

        [HttpGet]
        public IActionResult ReddedilenSiparisleriGetir(int id)
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Cancelled).ToList());
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public IActionResult SiparisSil(int id)
        {
            var order = _orderService.GetByID(id);
            if (order == null)
            {
                return NotFound();
            }

            try
            {
                _orderService.Remove(order);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok("Ürün Silindi!");
        }       

        // POST: api/Order
        [HttpPost]
        public IActionResult SiparisEkle(int userID, [FromQuery] int[] productIDs, [FromQuery] short[] quantities)
        {
            Order yeniSiparis = new Order();
            yeniSiparis.UserID = userID;
            yeniSiparis.Status = Status.Pending;
            yeniSiparis.IsActive = true;

            _orderService.Add(yeniSiparis); //DB'ye eklendiğinde ID oluşuyor.

            for (int i = 0; i < productIDs.Length; i++)
            {
                OrderDetails yeniDetay = new OrderDetails();
                yeniDetay.OrderID = yeniSiparis.ID;
                yeniDetay.ProductID = productIDs[i];
                yeniDetay.Quantity = quantities[i];
                yeniDetay.UnitPrice = _productService.GetByID(productIDs[i]).UnitPrice;
                yeniDetay.IsActive = true;

                _odService.Add(yeniDetay);
            }
            return Ok(yeniSiparis);
        }

        [HttpGet("{id}")]
        public IActionResult SiparisOnayla(int id)
        {
            Order confirmedOrder = _orderService.GetByID(id);
            if (confirmedOrder ==null)
            {
                return NotFound();
            }
            else
            {
                
                List<OrderDetails> detaylar = _odService.GetDefault(x=>x.OrderID == confirmedOrder.ID).ToList();
                foreach (OrderDetails item in detaylar)
                {
                    Product productInOrder = _productService.GetByID(item.ProductID);
                    productInOrder.Stock -= item.Quantity;
                    _productService.Update(productInOrder);
                }

                confirmedOrder.Status = Status.Confirmed;
                confirmedOrder.IsActive = false;
                _orderService.Update(confirmedOrder);
                return Ok(confirmedOrder);
            }
        }

        [HttpGet("{id}")]
        public IActionResult SiparisReddet(int id, Order order)
        {
            Order cancelledOrder = _orderService.GetByID(id);
            if (cancelledOrder == null)
            {
                return NotFound();
            }
            else
            {
                cancelledOrder.Status = Status.Cancelled;
                cancelledOrder.IsActive = false;
                _orderService.Update(cancelledOrder);
                return Ok(cancelledOrder);
            }
        }


    }
}
