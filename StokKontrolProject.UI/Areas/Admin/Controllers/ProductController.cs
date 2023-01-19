using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using System.Text;

namespace StokKontrolProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        string uri = "https://localhost:7207";
        public async Task<IActionResult> Index()
        {
            List<Product> urunler = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/TumUrunleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            return View(urunler);
        }

        [HttpGet]
        public async Task<IActionResult> UrunAktiflestir(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/UrunAktiflestir/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UrunSil(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Product/UrunSil/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UrunEkle()
        {
            IEnumerable<Category> aktifKategoriler = new List<Category>();
            IEnumerable<Supplier> aktifTedarikciler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifKategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifTedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }

            }
            @ViewBag.AktifKategoriler = aktifKategoriler;
            @ViewBag.AktifTedarikciler = aktifTedarikciler;
            return View(); // Sadece ekleme view ını gösterecek
        }

        [HttpPost]
        public async Task<IActionResult> UrunEkle(Product product)
        {
            product.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Product/UrunEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        static Product updatedProduct; // ilgili Producti güncelleme işleminin devamındaki (put) kullanacapımız için o metottan da ulaşabilmek adına globalde tanımlayalım. Eklenme tarihini güncellenm işleminde kullanmak için static hale getirdik
        //DateTime eklenmeTarihi = updatedProduct.AddedDate;
        [HttpGet]
        public async Task<IActionResult> UrunGuncelle(int id) //id ile ilgili kategoryi bul getir
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/IdyeGoreUrunGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedProduct = JsonConvert.DeserializeObject<Product>(apiCevap);
                }
            }
            return View(updatedProduct); //update edilecek urunyi güncelleme view'ında gösterecek  
        }

        [HttpPost]
        public async Task<IActionResult> UrunGuncelle(Product guncelUrun) //güncellenmiş urun parametre olarak alınır.
        {
            using (var httpClient = new HttpClient())
            {
                guncelUrun.AddedDate = updatedProduct.AddedDate;
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelUrun), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PutAsync($"{uri}/api/Product/UrunGuncelle/{guncelUrun.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
