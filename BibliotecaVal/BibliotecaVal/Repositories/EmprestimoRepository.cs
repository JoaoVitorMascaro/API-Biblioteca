using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories
{
    // Responsável por fazer as operações de empréstimos no banco de dados
    public class EmprestimoRepository
    {
        // Guarda o acesso ao banco através do AppDbContext
        private readonly AppDbContext _context;

        public EmprestimoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Busca todos os empréstimos cadastrados
        public async Task<List<Emprestimo>> GetTodosAsync()
        {
            // Include também traz os dados do livro relacionado ao empréstimo
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .ToListAsync();
        }

        // Busca um empréstimo específico pelo ID
        public async Task<Emprestimo?> GetPorIdAsync(Guid id)
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Adiciona um novo empréstimo no banco
        public async Task AdicionarAsync(Emprestimo emprestimo)
        {
            await _context.Emprestimos.AddAsync(emprestimo);
            await _context.SaveChangesAsync();
        }

        // Atualiza um empréstimo que já existe
        public async Task AtualizarAsync(Emprestimo emprestimo)
        {
            _context.Emprestimos.Update(emprestimo);
            await _context.SaveChangesAsync();
        }
    }
}
