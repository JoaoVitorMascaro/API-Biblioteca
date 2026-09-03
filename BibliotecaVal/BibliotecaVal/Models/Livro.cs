using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models
{
    public class Livro
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Autor { get; set; } = string.Empty;

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; } = string.Empty;

        public bool Disponivel { get; set; } = true;

        public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    }
}

