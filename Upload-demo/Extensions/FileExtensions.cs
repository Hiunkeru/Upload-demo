using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Upload_demo.Extensions
{
    public static class FileExtensions
    {
        public static byte[] ConvertToByteArray(this IFormFile content)
        {
            using (var stream = new MemoryStream())
            {
                content.CopyTo(stream);
                return stream.ToArray();
            }
        }

        public static string Base64Url(this byte[] content)
        {
            return $"data:image/{content.FileExtension()};base64,{Convert.ToBase64String(content)}";
        }

        public static string FileExtension(this byte[] content)
        {
            var base64 = Convert.ToBase64String(content).Substring(0, 5);

            return (base64.ToUpper()) switch
            {
                "IVBOR" => "png",
                "/9J/4" => "jpg",
                _ => string.Empty,
            };
        }
    }
}
