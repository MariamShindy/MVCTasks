using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace TaskThree.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
            //string FolderPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\{FolderName}";
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            string FileName = $"{Guid.NewGuid()} {Path.GetExtension(file.FileName)}";

            string FilePath = Path.Combine(FolderPath, FileName);
            using var fileStream = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(fileStream);
            return FileName;
        }
        public static void DeleteFile(string FileName , string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files" ,FolderName, FileName);
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
