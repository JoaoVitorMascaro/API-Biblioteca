using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Data
{
    // Essa classe faz a ligação entre a API e o banco de dados
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Livro> Livros { get; set; }

        // Representa a tabela de empréstimos no banco
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define que o ISBN não pode se repetir no banco
            modelBuilder.Entity<Livro>()
                .HasIndex(l => l.ISBN)
                .IsUnique();

            // Cria uma relação entre Livro e Emprestimo
            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Livro)
                .WithMany(l => l.Emprestimos)
                .HasForeignKey(e => e.LivroId);
        }
    }
}
