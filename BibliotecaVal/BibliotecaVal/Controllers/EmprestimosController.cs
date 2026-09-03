
using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
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


        // Retorna todos os empréstimos cadastrados
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var emprestimos = await _emprestimoService.GetTodosAsync();

            return Ok(emprestimos);
        }

        
        // Busca um empréstimo específico pelo seu ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(Guid id)
        {
            var emprestimo = await _emprestimoService.GetPorIdAsync(id);

   
            if (emprestimo == null)
                return NotFound(new { mensagem = "Empréstimo não encontrado." });

            return Ok(emprestimo);
        }

        // POST - Cria um novo empréstimo
        [HttpPost]
        public async Task<IActionResult> Criar(Emprestimo emprestimo)
        {
            try
            {  
                var novoEmprestimo =
                    await _emprestimoService.CriarAsync(emprestimo);

                return CreatedAtAction(
                    nameof(GetPorId),
                    new { id = novoEmprestimo.Id },
                    novoEmprestimo
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        // PUT: api/emprestimos/{id}/devolver
        [HttpPut("{id}/devolver")]
        public async Task<IActionResult> Devolver(Guid id)
        {
            try
            {
                var emprestimo =
                    await _emprestimoService.DevolverAsync(id);

                // Se o empréstimo não existir, retorna erro
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
