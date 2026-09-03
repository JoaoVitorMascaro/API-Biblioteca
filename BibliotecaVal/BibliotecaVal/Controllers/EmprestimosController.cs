
using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    // Define que esse controller vai cuidar das rotas relacionadas aos empréstimos
    [ApiController]
    [Route("api/emprestimos")]
    public class EmprestimosController : ControllerBase
    {
        // Guarda o Service que tem as regras e operações dos empréstimos
        private readonly EmprestimoService _emprestimoService;

        // Recebe o Service pelo construtor para poder usar seus métodos
        public EmprestimosController(EmprestimoService emprestimoService)
        {
            _emprestimoService = emprestimoService;
        }


        // GET: api/emprestimos
        // Retorna todos os empréstimos cadastrados
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var emprestimos = await _emprestimoService.GetTodosAsync();

            return Ok(emprestimos);
        }

        // GET: api/emprestimos/{id}
        // Busca um empréstimo específico pelo seu ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(Guid id)
        {
            var emprestimo = await _emprestimoService.GetPorIdAsync(id);

            // Se não encontrar o empréstimo, retorna erro 404
            if (emprestimo == null)
                return NotFound(new { mensagem = "Empréstimo não encontrado." });

            return Ok(emprestimo);
        }

        // POST: api/emprestimos
        // Cria um novo empréstimo
        [HttpPost]
        public async Task<IActionResult> Criar(Emprestimo emprestimo)
        {
            try
            {
                // Envia o empréstimo para o Service fazer o cadastro
                var novoEmprestimo =
                    await _emprestimoService.CriarAsync(emprestimo);

                // Retorna 201 informando que o empréstimo foi criado
                return CreatedAtAction(
                    nameof(GetPorId),
                    new { id = novoEmprestimo.Id },
                    novoEmprestimo
                );
            }
            catch (Exception ex)
            {
                // Se acontecer algum erro, retorna 400 com a mensagem do erro
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // PUT: api/emprestimos/{id}/devolver
        // Marca um empréstimo como devolvido
        [HttpPut("{id}/devolver")]
        public async Task<IActionResult> Devolver(Guid id)
        {
            try
            {
                // Chama o Service para realizar a devolução do livro
                var emprestimo =
                    await _emprestimoService.DevolverAsync(id);

                // Se o empréstimo não existir, retorna 404
                if (emprestimo == null)
                    return NotFound(new { mensagem = "Empréstimo não encontrado." });

                return Ok(emprestimo);
            }
            catch (Exception ex)
            {
                // Caso dê algum problema, retorna 400 com a mensagem
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
