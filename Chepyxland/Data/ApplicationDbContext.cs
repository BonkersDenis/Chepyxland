using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Chepyxland.Models;

namespace Chepyxland.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FileEntity> Files { get; set; }// Коллекция файлов в базе данных

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)// Передаем параметры конфигурации базы данных
        {
            this.Database.EnsureCreated();
        }
    }
}
