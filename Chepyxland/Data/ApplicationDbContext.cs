using Microsoft.EntityFrameworkCore;
using Chepyxland.Models;

namespace Chepyxland.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):DbContext(options)
    {
        public DbSet<FileEntity> Files { get; set; }// Коллекция файлов в базе данных
        
        public DbSet<User> Users { get; set; }// Коллекция файлов в базе данных

        // Передаем параметры конфигурации базы данных
    }

}
