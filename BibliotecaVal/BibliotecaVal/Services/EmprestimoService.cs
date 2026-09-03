using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;

namespace BibliotecaAPI.Services
{
    // É aqui que ficam as regras relacionadas aos empréstimos
    public class EmprestimoService
    {
        // Repositories usados para acessar os dados dos empréstimos e livros
        private readonly EmprestimoRepository _emprestimoRepository;
        private readonly LivroRepository _livroRepository;

        public EmprestimoService(
            EmprestimoRepository emprestimoRepository,
            LivroRepository livroRepository)
        {
            _emprestimoRepository = emprestimoRepository;
            _livroRepository = livroRepository;
        }

        // Busca todos os empréstimos
        public async Task<List<Emprestimo>> GetTodosAsync()
        {
            return await _emprestimoRepository.GetTodosAsync();
        }

        // Busca um empréstimo pelo ID
        public async Task<Emprestimo?> GetPorIdAsync(Guid id)
        {
            return await _emprestimoRepository.GetPorIdAsync(id);
        }

        // Cria um novo empréstimo
        public async Task<Emprestimo> CriarAsync(Emprestimo emprestimo)
        {
            // Primeiro verifica se o livro existe
            var livro = await _livroRepository.GetPorIdAsync(
                emprestimo.LivroId);

            if (livro == null)
            {
                throw new Exception("Livro não encontrado.");
            }

            // Não permite emprestar um livro que já está emprestado
            if (!livro.Disponivel)
            {
                throw new Exception("Livro já está emprestado.");
            }

            // Define os dados do novo empréstimo
            emprestimo.Id = Guid.NewGuid();
            emprestimo.DataEmprestimo = DateTime.UtcNow;
            emprestimo.DataDevolucao = null;

            // Depois do empréstimo, o livro fica indisponível
            livro.Disponivel = false;

            // Salva o empréstimo e atualiza o status do livro
            await _emprestimoRepository.AdicionarAsync(emprestimo);
            await _livroRepository.AtualizarAsync(livro);

            return emprestimo;
        }

        // Realiza a devolução de um livro
        public async Task<Emprestimo?> DevolverAsync(Guid id)
        {
            // Procura o empréstimo pelo ID
            var emprestimo =
                await _emprestimoRepository.GetPorIdAsync(id);

            if (emprestimo == null)
            {
                return null;
            }

            // Verifica se o empréstimo já foi devolvido
            if (emprestimo.DataDevolucao != null)
            {
                throw new Exception("Este empréstimo já foi devolvido.");
            }

            // Registra a data da devolução
            emprestimo.DataDevolucao = DateTime.UtcNow;

            // Quando devolve, o livro volta a ficar disponível
            if (emprestimo.Livro != null)
            {
                emprestimo.Livro.Disponivel = true;

                await _livroRepository.AtualizarAsync(
                    emprestimo.Livro);
            }

            // Atualiza o empréstimo no banco
            await _emprestimoRepository.AtualizarAsync(emprestimo);

            return emprestimo;
        }
    }
}
