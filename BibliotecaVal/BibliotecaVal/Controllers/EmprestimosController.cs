using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/emprestimos")]
    public class EmprestimosController : ControllerBase
    {
        private readonly EmprestimoService _emprestimoService;

        public EmprestimosController(EmprestimoService emprestimoService)
        {
            _emprestimoService = emprestimoService;
        }


        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var emprestimos = await _emprestimoService.GetTodosAsync();

            return Ok(emprestimos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(Guid id)
        {
            var emprestimo = await _emprestimoService.GetPorIdAsync(id);

            if (emprestimo == null)
                return NotFound(new { mensagem = "Empréstimo não encontrado." });

            return Ok(emprestimo);
        }

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

        [HttpPut("{id}/devolver")]
        public async Task<IActionResult> Devolver(Guid id)
        {
            try
            {
                var emprestimo =
                    await _emprestimoService.DevolverAsync(id);

                if (emprestimo == null)
                    return NotFound(new { mensagem = "Empréstimo não encontrado." });

                return Ok(emprestimo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}