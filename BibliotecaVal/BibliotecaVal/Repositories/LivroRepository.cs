using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories
{
    public class LivroRepository
    {
        private readonly AppDbContext _context;

        public LivroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Livro>> GetTodosAsync()
        {
            return await _context.Livros
                .Include(l => l.Emprestimos)
                .ToListAsync();
        }

        public async Task<Livro?> GetPorIdAsync(Guid id)
        {
            return await _context.Livros
                .Include(l => l.Emprestimos)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Livro?> GetPorISBNAsync(string isbn)
        {
            return await _context.Livros
                .FirstOrDefaultAsync(l => l.ISBN == isbn);
        }

        public async Task AdicionarAsync(Livro livro)
        {
            await _context.Livros.AddAsync(livro);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Livro livro)
        {
            _context.Livros.Update(livro);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(Livro livro)
        {
            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();
        }
    }
}