using DistribuidoraDoChines.Commons.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace DistribuidoraDoChines.Api.Data.Context
{
    public class DistribuidoraDoChinesContext : DbContext
    {
        public DistribuidoraDoChinesContext()
        {
        }

        public DistribuidoraDoChinesContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<Detalhes> Detalhes { get; set; }
        public virtual DbSet<Efmigrationshistory> Efmigrationshistory { get; set; }
        public virtual DbSet<Enderecos> Enderecos { get; set; }
        public virtual DbSet<Pedidos> Pedidos { get; set; }
        public virtual DbSet<Produtos> Produtos { get; set; }
        public virtual DbSet<Telefones> Telefones { get; set; }
        public virtual DbSet<TiposDePagamento> TiposDePagamento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("usuarios");

                entity.HasIndex(e => e.Usuario)
                    .HasName("usuario")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasColumnName("senha")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<Categorias>(entity =>
            {
                entity.ToTable("categorias");

                entity.HasIndex(e => e.Descricao)
                    .HasName("descricao")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.ToTable("clientes");

                entity.HasIndex(e => e.Email)
                    .HasName("email")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasColumnName("senha")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Detalhes>(entity =>
            {
                entity.ToTable("detalhes");

                entity.HasIndex(e => e.IdPedido)
                    .HasName("Detalhes_fk0");

                entity.HasIndex(e => e.IdProduto)
                    .HasName("Detalhes_fk1");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");


                entity.Property(e => e.IdPedido).HasColumnName("id_pedido");

                entity.Property(e => e.IdProduto).HasColumnName("id_produto");

                entity.Property(e => e.Quantidade).HasColumnName("quantidade");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(10,2) unsigned");

                entity.HasOne(d => d.IdPedidoNavigation)
                    .WithMany(p => p.Detalhes)
                    .HasForeignKey(d => d.IdPedido)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("detalhes_ibfk_1");

                entity.HasOne(d => d.IdProdutoNavigation)
                    .WithMany(p => p.Detalhes)
                    .HasForeignKey(d => d.IdProduto)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("detalhes_ibfk_2");
            });

            modelBuilder.Entity<Efmigrationshistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId)
                    .HasName("PRIMARY");

                entity.ToTable("__efmigrationshistory");

                entity.Property(e => e.MigrationId).HasColumnType("varchar(95)");

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<Enderecos>(entity =>
            {
                entity.ToTable("enderecos");

                entity.HasIndex(e => e.IdCliente)
                    .HasName("Enderecos_fk0");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bairro)
                    .IsRequired()
                    .HasColumnName("bairro")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Cep)
                    .IsRequired()
                    .HasColumnName("cep")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Complemento)
                    .HasColumnName("complemento")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.Nome)
                    .HasColumnName("nome")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasColumnName("numero")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Referencia)
                    .HasColumnName("referencia")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Rua)
                    .IsRequired()
                    .HasColumnName("rua")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Enderecos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("enderecos_ibfk_1");
            });

            modelBuilder.Entity<Pedidos>(entity =>
            {
                entity.ToTable("pedidos");

                entity.HasIndex(e => e.IdCliente)
                    .HasName("Pedidos_fk0");

                entity.HasIndex(e => e.IdClienteEndereco)
                    .HasName("Pedidos_fk1");

                entity.HasIndex(e => e.IdTiposDePagamento)
                    .HasName("Pedidos_fk2");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.IdClienteEndereco).HasColumnName("id_cliente_endereco");

                entity.Property(e => e.IdTiposDePagamento).HasColumnName("id_tipos_de_pagamento");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasColumnType("enum('Novo',''Em Atendimento', 'Finalizado')")
                    .HasDefaultValueSql("'Novo'");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Valor)
                    .HasColumnName("valor")
                    .HasColumnType("decimal(10,2) unsigned");

                entity.Property(e => e.ValorFrete)
                    .HasColumnName("valor_frete")
                    .HasColumnType("decimal(10,2) unsigned");

                entity.Property(e => e.Troco)
                    .HasColumnName("troco")
                    .HasColumnType("decimal(10,2) unsigned");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("pedidos_ibfk_1");

                entity.HasOne(d => d.IdClienteEnderecoNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdClienteEndereco)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("pedidos_ibfk_2");

                entity.HasOne(d => d.IdTiposDePagamentoNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.IdTiposDePagamento)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("pedidos_ibfk_3");
            });

            modelBuilder.Entity<Produtos>(entity =>
            {
                entity.ToTable("produtos");

                entity.HasIndex(e => e.IdCategoria)
                    .HasName("Produtos_fk0");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");

                entity.Property(e => e.Imagem).HasColumnName("imagem");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Preco)
                    .HasColumnName("preco")
                    .HasColumnType("decimal(10,2) unsigned");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Unidade).HasColumnName("unidade");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Produtos)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("produtos_ibfk_1");
            });

            modelBuilder.Entity<Telefones>(entity =>
            {
                entity.ToTable("telefones");

                entity.HasIndex(e => e.IdCliente)
                    .HasName("Telefones_fk0");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Celular)
                    .HasColumnName("celular")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Comercial)
                    .HasColumnName("comercial")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.IdCliente).HasColumnName("id_cliente");

                entity.Property(e => e.Residencial)
                    .HasColumnName("residencial")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Telefones)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("telefones_ibfk_1");
            });

            modelBuilder.Entity<TiposDePagamento>(entity =>
            {
                entity.ToTable("tipos_de_pagamento");

                entity.HasIndex(e => e.Descricao)
                    .HasName("descricao")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("descricao")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();
            });
        }
    }
}