using Microsoft.AspNetCore.Mvc;
using Chepyxland.Data; // Пространство имен для ApplicationDbContext
using Chepyxland.Models; // Пространство имен для FileEntity
using Microsoft.AspNetCore.Http; // Для работы с IFormFile
using System.IO;
using System.Threading.Tasks;

namespace Chepyxland.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context; // Контекст базы данных

        // Конструктор контроллера
        public FilesController(ApplicationDbContext context)
        {
            _context = context; // Инъекция контекста базы данных
        }

        // GET: Files/Index
        public IActionResult Index()
        {
            // Получаем все файлы из базы данных
            var files = _context.Files.ToList();
            return View(files); // Передаем список файлов во view
        }

        // GET: Files/Upload
        public IActionResult Upload()
        {
            // Отображаем форму для загрузки файла
            return View();
        }

        // POST: Files/Upload
        [HttpPost]
        //TODO: Настроить токен на стороне клиента. [ValidateAntiForgeryToken] // Защита от CSRF-атак
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                // Если файл не выбран, показываем сообщение об ошибке
                return Content("No file selected.");
            }

            // Определяем путь для сохранения файла
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");

            // Проверяем, существует ли папка uploads, и создаем её, если её нет
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Генерируем безопасное имя файла
            string fileName = Path.GetFileNameWithoutExtension(file.FileName); // Берем имя без расширения
            string fileExtension = Path.GetExtension(file.FileName); // Берем расширение
            fileName = fileName.Replace(" ", "_"); // Заменяем пробелы на подчеркивания
            string safeFileName = $"{fileName}{fileExtension}"; // Создаем безопасное имя файла

            var filePath = Path.Combine(uploadsFolder, safeFileName);

            // Сохраняем файл на диск
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Создаем запись о файле в базе данных
            var fileEntity = new FileEntity
            {
                FileName = safeFileName,
                FilePath = $"/uploads/{safeFileName}", // Относительный путь для веб-сервера
                ContentType = file.ContentType,
                UploadDate = DateTime.Now,
                UploadedBy = User.Identity.Name ?? "Anonymous" // Имя пользователя или "Anonymous"
            };

            _context.Files.Add(fileEntity); // Добавляем файл в базу данных
            await _context.SaveChangesAsync(); // Сохраняем изменения

            return RedirectToAction(nameof(Index)); // Перенаправление на страницу со списком файлов
        }
        // GET: Files/Download/{id}
        public IActionResult Download(int id)
        {
            // Находим файл по ID
            var file = _context.Files.Find(id);
            if (file == null)
            {
                return NotFound(); // Файл не найден
            }

            // Определяем полный путь к файлу
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FilePath.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Файл отсутствует на диске
            }

            // Читаем содержимое файла в поток
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            // Возвращаем файл клиенту
            return File(memory, file.ContentType, file.FileName);
        }
    }
}