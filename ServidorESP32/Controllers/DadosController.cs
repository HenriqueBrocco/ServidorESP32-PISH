using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

namespace ServidorESP32.Controllers
{
    public class Dado
    {
        public int Id { get; set; }
        public int Bpm { get; set; }
        public int Oxigenacao { get; set; }
        public bool Queda { get; set; }
        public string? Coordenadas { get; set; }
        public string? CoordenadasMaps { get; set; }
        public DateTime DataHora { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DadosController : Controller
    {
        private static List<Dado> dadosRecebidos = new();

        [HttpPost]
        public IActionResult Receber([FromBody] JsonElement json)
        {
            var cultureInfo = new CultureInfo("pt-BR");
            var id = json.GetProperty("Id").GetString();
            var bpm = json.GetProperty("Freq").GetString();
            var oxigenacao = json.GetProperty("Oxigenacao").GetString();
            var queda = json.GetProperty("Queda").GetString();
            var dataHora = json.GetProperty("DataHora").GetString();
            var coordenadas = json.GetProperty("Coordenadas").GetString();
            
            if (id == "2") 
            {
                dadosRecebidos = new();
            }
            else { 
                dadosRecebidos.Add(new Dado
                {
                    Id = Int32.Parse(id ?? "0"),
                    Bpm = Int32.Parse(bpm ?? "0"),
                    Oxigenacao = Int32.Parse(oxigenacao ?? "0"),
                    Queda = Boolean.Parse(queda == "1" ? "true" : "false"),
                    Coordenadas = coordenadas,
                    DataHora = DateTime.Parse(dataHora ?? "01/01/2025 10:04:00", cultureInfo, DateTimeStyles.NoCurrentDateDefault),
                    CoordenadasMaps = coordenadas == "" ? "" : coordenadas.Split(" - ")[0]
                });
            }
            return Ok(new { status = "ok" });
        }

        [HttpGet]
        [Route("/visualizar")]
        public IActionResult Visualizar()
        {
            return View("Visualizar", dadosRecebidos);
        }
    }
}