using Microsoft.EntityFrameworkCore; // ��� ������ � DbContext
using Chepyxland.Data; // ������������ ����, ��� ��������� ApplicationDbContext

namespace Chepyxland
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. ����������� ����� � ���������� ������������
            // ��������� ��������� MVC
            builder.Services.AddControllersWithViews();

            // ������������ �������� ���� ������
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. ������� ����������
            var app = builder.Build();

            // 3. ��������� HTTP-���������
            if (!app.Environment.IsDevelopment())
            {
                // ��������� ������ � ���������-�����
                app.UseExceptionHandler("/Home/Error"); // ��������������� �� �������� ������
                app.UseHsts(); // ��������� HSTS ��� HTTPS
            }

            app.UseHttpsRedirection(); // ��������������� �� HTTPS
            app.UseStaticFiles(); // ��������� ����������� ������ (css, js, uploads)

            app.UseRouting(); // ��������� �������������

            app.UseAuthorization(); // �����������

            // 4. ��������� ��������� ��� MVC
            app.MapControllerRoute(
                name: "default", // ��� ��������
                pattern: "{controller=Files}/{action=Index}/{id?}"); // ������ ��������

            // 5. ������ ����������
            app.Run();
        }
    }
}