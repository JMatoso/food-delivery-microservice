using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Product.Services
{
    public static class UploadFile
    {
        private static IWebHostEnvironment _web { get; set; }

        public static async Task<string> Upload(IFormFile image)
        {
            string uniqueFileName = null;

            if(image != null)
            {
                string path = Path.Combine(_web.WebRootPath, "images/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(path, uniqueFileName);

                using(var fs = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fs);
                }
            }

            return uniqueFileName;
        }
    }
}