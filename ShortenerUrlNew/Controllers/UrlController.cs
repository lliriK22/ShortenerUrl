using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortenerUrlNew.Functions;
using ShortenerUrlNew.Models;
using ShortenerUrlNew.Data;

namespace ShortenerUrlNew.Controllers
{
    public class UrlController : Controller
    {
        private readonly AppContextDb db;
        public UrlController(AppContextDb _db)
        {
            this.db = _db;
        }

        public IActionResult Index()
        {
            IEnumerable<Url> list_obj = db.urlList.ToList();

            return View(list_obj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Url br = new Url();
            return PartialView(br);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Url obj)
        {
            if (obj == null || obj.longUrl == null || !obj.longUrl.Contains("https://"))
            {
                ModelState.AddModelError(nameof(obj.longUrl), "Некорректные входные данные");
                return View(obj);
            }

            if (ModelState.IsValid)
            {
                do //Генерация короткого URL; при совпадении, цикл продолжает работу
                {
                    obj.shortUrl = CreateShortUrl.Create(Request.Host.ToString(), "/Url/RedTo/");
                }
                while (db.urlList.FirstOrDefault(x => x.shortUrl == obj.shortUrl) != null);

                obj.dateTime = DateTime.Now;//Дата создания
                obj.countTransitionUrl = 0;//Кол-во переходов

                db.urlList.Add(obj);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var obj = db.urlList.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Url obj)
        {
            if (obj == null)
                return NotFound();

            do
            {
                obj.shortUrl = CreateShortUrl.Create(Request.Host.ToString(), "/Url/RedTo/");
            }
            while (db.urlList.FirstOrDefault(x => x.shortUrl == obj.shortUrl) != null);

            obj.dateTime = DateTime.Now;
            obj.countTransitionUrl = 0;

            db.urlList.Update(obj);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var obj = db.urlList.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            return PartialView(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int? id)
        {
            var obj = db.urlList.FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            db.urlList.Remove(obj);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Перенаправление через оригинальную ссылку
        [HttpGet("/Url/RedTo/{path:required}")]
        public IActionResult RedTo(string path)
        {
            if (path == null)
            {
                return NotFound();
            }

            path = "https://" + Request.Host + "/Url/RedTo/" + path;

            var originalUrl = db.urlList.FirstOrDefault(x => x.shortUrl == path);

            if (originalUrl == null)
            {
                return NotFound();
            }

            originalUrl.countTransitionUrl++;

            db.urlList.Update(originalUrl);
            db.SaveChanges();

            return Redirect(originalUrl.longUrl);
        }
    }
}
