using System.Diagnostics;
using DW01.Models;
using Microsoft.AspNetCore.Mvc;

namespace DW01.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Professor()
        {
            return View();
        }
        public IActionResult Produto()
        {
            return View();
        }
        public IActionResult DetalhesProduto(int id)
        {
            return Content($"Produto recebido: {id}");
        }
        
        [HttpGet]
        public IActionResult CriarProduto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CriarProduto(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return View(produto);
            }

            return Content($"Produto {produto.Nome} válido!");
        }

        public IActionResult BuscarProduto(string? nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                return View();
            }

            return Content($"Produto pesquisado: {nome}");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
