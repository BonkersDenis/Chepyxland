using Microsoft.EntityFrameworkCore; // Для работы с DbContext
using Chepyxland.Data; // Пространство имен, где находится ApplicationDbContext

namespace Chepyxland
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Регистрация служб в контейнере зависимостей
            // Добавляем поддержку MVC
            builder.Services.AddControllersWithViews();

            // Регистрируем контекст базы данных
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Создаем приложение
            var app = builder.Build();

            // 3. Настройка HTTP-конвейера
            if (!app.Environment.IsDevelopment())
            {
                // Обработка ошибок в продакшен-среде
                app.UseExceptionHandler("/Home/Error"); // Перенаправление на страницу ошибки
                app.UseHsts(); // Включение HSTS для HTTPS
            }

            app.UseHttpsRedirection(); // Перенаправление на HTTPS
            app.UseStaticFiles(); // Поддержка статических файлов (css, js, uploads)

            app.UseRouting(); // Настройка маршрутизации

            app.UseAuthorization(); // Авторизация

            // 4. Настройка маршрутов для MVC
            app.MapControllerRoute(
                name: "default", // Имя маршрута
                pattern: "{controller=Files}/{action=Index}/{id?}"); // Шаблон маршрута

            // 5. Запуск приложения
            app.Run();
        }
    }
}