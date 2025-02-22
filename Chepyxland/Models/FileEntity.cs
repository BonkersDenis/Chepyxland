namespace Chepyxland.Models
{
    public class FileEntity
    {
        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        public int Id { get; set; }
        //TODO: Переделать комментарии под summary
        public string FileName { get; set; } // Имя файла
        public string FilePath { get; set; } // Путь к файлу
        public string ContentType { get; set; } // Тип файла (например, "application/pdf")
        public DateTime UploadDate { get; set; } // Дата загрузки
        public string UploadedBy { get; set; } // Кто загрузил файл
    }
}
