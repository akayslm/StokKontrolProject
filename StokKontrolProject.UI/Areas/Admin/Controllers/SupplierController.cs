using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using System.Text;

namespace StokKontrolProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        string uri = "https://localhost:7207";
        public async Task<IActionResult> Index()
        {
            List<Supplier> tedarikciler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TumTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return View(tedarikciler);
        }

        [HttpGet]
        public async Task<IActionResult> TedarikciAktiflestir(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TedarikciAktiflestir/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> TedarikciSil(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Supplier/TedarikciSil/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult TedarikciEkle()
        {
            return View(); // Sadece ekleme view ını gösterecek
        }

        [HttpPost]
        public async Task<IActionResult> TedarikciEkle(Supplier supplier)
        {
            supplier.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Supplier/TedarikciEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }

        static Supplier updatedSupplier; // ilgili Supplieri güncelleme işleminin devamındaki (put) kullanacapımız için o metottan da ulaşabilmek adına globalde tanımlayalım. Eklenme tarihini güncellenm işleminde kullanmak için static hale getirdik
        //DateTime eklenmeTarihi = updatedSupplier.AddedDate;
        [HttpGet]
        public async Task<IActionResult> TedarikciGuncelle(int id) //id ile ilgili kategoryi bul getir
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/IdyeGoreTedarikciGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedSupplier = JsonConvert.DeserializeObject<Supplier>(apiCevap);
                }
            }
            return View(updatedSupplier); //update edilecek tedarikciyi güncelleme view'ında gösterecek  
        }

        [HttpPost]
        public async Task<IActionResult> TedarikciGuncelle(Supplier guncelTedarikci) //güncellenmiş tedarikci parametre olarak alınır.
        {
            using (var httpClient = new HttpClient())
            {
                guncelTedarikci.AddedDate = updatedSupplier.AddedDate;
                guncelTedarikci.IsActive = true;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelTedarikci), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PutAsync($"{uri}/api/Supplier/TedarikciGuncelle/{guncelTedarikci.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
