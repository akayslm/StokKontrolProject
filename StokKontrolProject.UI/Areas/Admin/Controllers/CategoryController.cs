using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using System.Collections.Generic;
using System.Text;

namespace StokKontrolProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        string uri = "https://localhost:7207";
        public async Task<IActionResult> Index()
        {
            List<Category> kategoriler = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/TumKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return View(kategoriler);
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
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Category/TumKategorileriGetir", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
