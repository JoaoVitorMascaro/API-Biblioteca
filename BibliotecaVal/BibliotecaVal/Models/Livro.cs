using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models
{
    public class Livro
    {
        public Guid Id { get; set; }

        // Título do livro
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        // Nome do autor
        [Required]
        [StringLength(150)]
        public string Autor { get; set; } = string.Empty;

        // ISBN
        [Required]
        [StringLength(13)]
        public string ISBN { get; set; } = string.Empty;

        // Indica se o livro está disponivel
        public bool Disponivel { get; set; } = true;

        // Guarda os empréstimos relacionados a esse livro
        public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    }
}

