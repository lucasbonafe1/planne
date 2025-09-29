using Microsoft.EntityFrameworkCore;
using Planne.Core.Entities;
using Planne.Core.Enums;

namespace Planne.UI.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<Board> Boards { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<TaskCard> TaskCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Board ---
        modelBuilder.Entity<Board>().HasData(
            new Board { Id = 1, Name = "Projeto Faculdade" },
            new Board { Id = 2, Name = "Kanban Pessoal" }
        );

        // --- Columns ---
        modelBuilder.Entity<Column>().HasData(
            new Column { Id = 1, Name = "A Fazer", BoardId = 1 },
            new Column { Id = 2, Name = "Em Progresso", BoardId = 1 },
            new Column { Id = 3, Name = "Concluído", BoardId = 1 },

            new Column { Id = 4, Name = "Backlog", BoardId = 2 },
            new Column { Id = 5, Name = "Doing", BoardId = 2 },
            new Column { Id = 6, Name = "Done", BoardId = 2 }
        );

        // --- TaskCards ---
        modelBuilder.Entity<TaskCard>().HasData(
            new TaskCard { Id = 1, Name = "Estudar Blazor", Description = "Assistir aulas e praticar componentes.", Priority = PriorityEnum.High, ColumnId = 1 },
            new TaskCard { Id = 2, Name = "Ler artigo sobre DDD", Description = "Focar em modelagem de domínio.", Priority = PriorityEnum.Normal, ColumnId = 1 },
            new TaskCard { Id = 3, Name = "Criar protótipo UI", Description = "Montar wireframe no Figma.", Priority = PriorityEnum.Low, ColumnId = 2 },
            new TaskCard { Id = 4, Name = "Revisar código", Description = "Refatorar services e componentes.", Priority = PriorityEnum.High, ColumnId = 2 },
            new TaskCard { Id = 5, Name = "Apresentar projeto", Description = "Preparar slides e demo.", Priority = PriorityEnum.Normal, ColumnId = 3 },

            new TaskCard { Id = 6, Name = "Planejar viagem", Description = "Pesquisar destinos e custos.", Priority = PriorityEnum.Low, ColumnId = 4 },
            new TaskCard { Id = 7, Name = "Organizar estudos ENEM", Description = "Criar cronograma semanal.", Priority = PriorityEnum.High, ColumnId = 5 },
            new TaskCard { Id = 8, Name = "Limpar casa", Description = "Organizar quarto e sala.", Priority = PriorityEnum.Normal, ColumnId = 5 },
            new TaskCard { Id = 9, Name = "Treinar corrida", Description = "Correr 5km na praça.", Priority = PriorityEnum.Normal, ColumnId = 6 }
        );
    }
}
