using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Upload_demo.Extensions;
using Upload_demo.Models;
using Upload_demo.Request;

namespace Upload_demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Consumes("multipart/form-data")]
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> UploadFile([FromForm]ProductosRequest request)
        {
            string fileExtension = Path.GetExtension(request.Fichero.FileName);

            if (fileExtension == ".xls" || fileExtension == ".xlsx")
            {
                return Ok(await ProcessExcel(request));
            }

            return Ok(await ProcessFile(request));

        }

        private async Task<IEnumerable<Productos>> ProcessExcel(ProductosRequest request)
        {
            List<Productos> productos = new List<Productos>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                request.Fichero.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each row of the file
                    {
                        productos.Add(new Productos { Categoria = reader.GetValue(0).ToString(), Nombre = reader.GetValue(1).ToString() });
                    }
                }
            }

            return productos;
        }

        private async Task<string> ProcessFile(ProductosRequest request)
        {
            return request.Fichero?.ConvertToByteArray().Base64Url();
        }


    }
}
