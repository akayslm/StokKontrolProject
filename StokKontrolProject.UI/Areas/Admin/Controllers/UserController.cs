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
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/TumKullanicilariGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kullaniciler = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }
            return View(kullaniciler);
        }

        [HttpGet]
        public async Task<IActionResult> KullaniciAktiflestir(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/KullaniciAktiflestir/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KullaniciSil(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/User/KullaniciSil/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult KullaniciEkle()
        {
            return View(); // Sadece ekleme view ını gösterecek
        }

        [HttpPost]
        public async Task<IActionResult> KullaniciEkle(User user)
        {
            user.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/User/KullaniciEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        static User updatedUser;

        [HttpGet]
        public async Task<IActionResult> KullaniciGuncelle(int id) //id ile ilgili kategoryi bul getir
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/IdyeGoreKullaniciGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedUser = JsonConvert.DeserializeObject<User>(apiCevap);
                }
            }
            return View(updatedUser); // Update edilecek kategoriyi güncelleme View'ında gösterecek  
        }

        [HttpPost]
        public async Task<IActionResult> KullaniciGuncelle(User guncelKullanici) //güncellenmiş kategori parametre olarak alınır.
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    guncelKullanici.AddedDate = updatedUser.AddedDate;
                    guncelKullanici.IsActive = true;
                    guncelKullanici.Password = updatedUser.Password;

                    StringContent content = new StringContent(JsonConvert.SerializeObject(guncelKullanici), Encoding.UTF8, "application/json");
                    using (var cevap = await httpClient.PutAsync($"{uri}/api/User/KullaniciGuncelle/{guncelKullanici.ID}", content))
                    {
                        string apiCevap = await cevap.Content.ReadAsStringAsync();
                        //kullanicilar = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                    }
                }
            }
            
            return RedirectToAction("Index");
        }
    }
}

