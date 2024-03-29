﻿using Microsoft.AspNetCore.Http;

namespace ECommerceApp.Core.Helpers;

public static class FileHelper
{
    public static async Task<string> SaveFile(IFormFile file, string path)
    {
        var uniqueFileName = GenerateUniqueFileName(file.FileName);
        var filePath = GetFilePath(uniqueFileName, path);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return uniqueFileName;
    }

    private static string GenerateUniqueFileName(string originalFileName)
    {
        var fileName = Path.GetRandomFileName();
        var fileExtension = Path.GetExtension(originalFileName);
        return fileName + fileExtension;
    }

    private static string GetFilePath(string fileName, string path)
    {

        var imagesFolder = Path.Combine(path, "assets", "images");
        return Path.Combine(imagesFolder, fileName);
    }
}