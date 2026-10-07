using DW01.Data;
using DW01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DW01.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController( ApplicationDbContext context, ILogger<ProdutosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Consultando lista de produtos.");

            return View(await _context.Produtos.ToListAsync());
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _logger.LogInformation("Consultando produto com ID {Id}.", id);

            if (id == null)
            {
                _logger.LogWarning( "Tentativa de consultar produto sem informar o ID.");

                return NotFound();
            }

            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
            {
                _logger.LogWarning( "Produto com ID {Id} não encontrado.", id);

                return NotFound();
            }

            return View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Acessando formulário de criação de produto.");

            return View();
        }

        // POST: Produtos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Produto criado: ID {Id}, Nome {Nome}, Preço {Preco}.", produto.Id, produto.Nome,produto.Preco);

                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Tentativa de criar produto com dados inválidos.");

            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("Acessando edição do produto com ID {Id}.", id);

            if (id == null)
            {
                _logger.LogWarning("Tentativa de editar produto sem informar o ID.");
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                _logger.LogWarning("Produto com ID {Id} não encontrado para edição.", id);
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produtos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( int id, Produto produto)
        {
            if (id != produto.Id)
            {
                _logger.LogWarning( "ID da URL ({Id}) diferente do ID do produto ({ProdutoId}).", id, produto.Id);
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Update(produto);
                await _context.SaveChangesAsync();

                _logger.LogInformation( "Produto editado: ID {Id}, Nome {Nome}, Preço {Preco}.",
                                         produto.Id, produto.Nome, produto.Preco);
                return RedirectToAction(nameof(Index));
            }
            _logger.LogWarning("Tentativa de editar produto com dados inválidos. ID: {Id}.", id);

            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("Acessando exclusão do produto com ID {Id}.", id);

            if (id == null)
            {
                _logger.LogWarning("Tentativa de excluir produto sem informar o ID.");
                return NotFound();
            }
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produto == null)
            {
                _logger.LogWarning("Produto com ID {Id} não encontrado para exclusão.", id);
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                _logger.LogWarning("Tentativa de excluir produto inexistente. ID: {Id}.", id);
                return NotFound();
            }
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            _logger.LogInformation( "Produto excluído: ID {Id}, Nome {Nome}.", produto.Id, produto.Nome);

            return RedirectToAction(nameof(Index));
        }
    }
}