using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using System.Text;

namespace StokKontrolProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        string uri = "https://localhost:7207";
        public async Task<IActionResult> Index()
        {
            List<User> kullaniciler = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/TumKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kullaniciler = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }
            return View(kullaniciler);
        }

        [HttpGet]
        public async Task<IActionResult> KategoriAktiflestir(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/KategoriAktiflestir/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KategoriSil(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Category/KategoriSil/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult KategoriEkle()
        {
            return View(); // Sadece ekleme view ını gösterecek
        }

        [HttpPost]
        public async Task<IActionResult> KategoriEkle(Category category)
        {
            category.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Category/KategoriEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        static Category updatedCategory; // İlgili kategoriyi güncelleme işleminin devamındaki (put) kullanacapımız için o metottan da ulaşabilmek adına globalde tanımlayalım. Eklenme tarihini güncelleme işleminde kullanmak için static hale getirdik

        [HttpGet]
        public async Task<IActionResult> KategoriGuncelle(int id) //id ile ilgili kategoryi bul getir
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/IdyeGoreKategoriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedCategory = JsonConvert.DeserializeObject<Category>(apiCevap);
                }
            }
            return View(updatedCategory); // Update edilecek kategoriyi güncelleme View'ında gösterecek  
        }

        [HttpPost]
        public async Task<IActionResult> KategoriGuncelle(Category guncelKategori) //güncellenmiş kategori parametre olarak alınır.
        {
            using (var httpClient = new HttpClient())
            {
                guncelKategori.AddedDate = updatedCategory.AddedDate;
                guncelKategori.IsActive = true;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelKategori), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PutAsync($"{uri}/api/Category/KategoriGuncelle/{guncelKategori.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }
    }
}

