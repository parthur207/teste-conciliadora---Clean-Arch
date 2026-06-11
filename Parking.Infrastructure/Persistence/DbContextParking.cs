using Microsoft.EntityFrameworkCore;
using Parking.Domain.Entities;

namespace Parking.Infrastructure.Persistence;

public class DbContextParking : DbContext
{
    public DbContextParking(DbContextOptions<DbContextParking> options) : base(options) { }

    public DbSet<ClienteEntity> Clientes { get; set; }
    public DbSet<VeiculoEntity> Veiculos { get; set; }
    public DbSet<FaturaEntity> Faturas { get; set; }
    public DbSet<FaturaVeiculoEntity> FaturasVeiculos { get; set; }
    public DbSet<VeiculoHistoricoEntity> VeiculosHistorico { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<ClienteEntity>(e =>
        {
            e.ToTable("cliente", "public");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Nome).HasColumnName("nome").IsRequired().HasMaxLength(200);
            e.Property(x => x.Telefone).HasColumnName("telefone").HasMaxLength(20);
            e.Property(x => x.Endereco).HasColumnName("endereco").HasMaxLength(400);
            e.Property(x => x.Mensalista).HasColumnName("mensalista");
            e.Property(x => x.ValorMensalidade).HasColumnName("valor_mensalidade");
            e.Property(x => x.DataInclusao).HasColumnName("data_inclusao");
            e.HasIndex(x => new { x.Nome, x.Telefone }).IsUnique(false);
            e.HasMany(x => x.Veiculos).WithOne(x => x.Cliente!).HasForeignKey(x => x.ClienteId);
        });

        modelBuilder.Entity<VeiculoEntity>(e =>
        {
            e.ToTable("veiculo", "public");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Placa).HasColumnName("placa").IsRequired().HasMaxLength(8);
            e.Property(x => x.Modelo).HasColumnName("modelo").HasMaxLength(120);
            e.Property(x => x.Ano).HasColumnName("ano");
            e.Property(x => x.DataInclusao).HasColumnName("data_inclusao");
            e.Property(x => x.ClienteId).HasColumnName("cliente_id");
            e.HasIndex(x => x.Placa).IsUnique();
        });

        modelBuilder.Entity<FaturaEntity>(e =>
        {
            e.ToTable("fatura", "public");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Competencia).HasColumnName("competencia").IsRequired();
            e.Property(x => x.ClienteId).HasColumnName("cliente_id");
            e.Property(x => x.Valor).HasColumnName("valor");
            e.Property(x => x.CriadaEm).HasColumnName("criada_em");
            e.Property(x => x.Observacao).HasColumnName("observacao");
            e.HasMany(x => x.Veiculos).WithOne().HasForeignKey(x => x.FaturaId);
            e.HasIndex(x => new { x.ClienteId, x.Competencia }).IsUnique();
        });

        modelBuilder.Entity<FaturaVeiculoEntity>(e =>
        {
            e.ToTable("fatura_veiculo", "public");
            e.HasKey(x => new { x.FaturaId, x.VeiculoId });
            e.Property(x => x.FaturaId).HasColumnName("fatura_id");
            e.Property(x => x.VeiculoId).HasColumnName("veiculo_id");
        });

        modelBuilder.Entity<VeiculoHistoricoEntity>(e =>
        {
            e.ToTable("veiculo_historico", "public");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.VeiculoId).HasColumnName("veiculo_id");
            e.Property(x => x.ClienteId).HasColumnName("cliente_id");
            e.Property(x => x.DataInicio).HasColumnName("data_inicio");
            e.Property(x => x.DataFim).HasColumnName("data_fim");
            e.HasIndex(x => new { x.VeiculoId, x.DataFim });
        });
    }
}
