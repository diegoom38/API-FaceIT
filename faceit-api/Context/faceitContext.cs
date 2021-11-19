using faceitapi.Models;
using Microsoft.EntityFrameworkCore;

namespace faceitapi.Context
{
    public partial class faceitContext : DbContext
    {
        public faceitContext(DbContextOptions<faceitContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Anexo> Anexo { get; set; }
        public virtual DbSet<Candidato> Candidato { get; set; }
        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<Imagem> Imagem { get; set; }
        public virtual DbSet<Pessoa> Pessoa { get; set; }
        public virtual DbSet<PessoaFisica> PessoaFisica { get; set; }
        public virtual DbSet<PessoaJuridica> PessoaJuridica { get; set; }
        public virtual DbSet<PessoaSkill> PessoaSkill { get; set; }
        public virtual DbSet<Proposta> Proposta { get; set; }
        public virtual DbSet<PropostaSkill> PropostaSkill { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<TipoSkill> TipoSkill { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anexo>(entity =>
            {
                entity.Property(e => e.IDPessoa).ValueGeneratedNever();

                entity.Property(e => e.Nome).IsUnicode(false);

                entity.HasOne(d => d.IDPessoaNavigation)
                    .WithOne(p => p.Anexo)
                    .HasForeignKey<Anexo>(d => d.IDPessoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anexo_Pessoa");
            });

            modelBuilder.Entity<Candidato>(entity =>
            {
                entity.HasKey(e => new { e.IDProposta, e.IDPessoa });

                entity.HasOne(d => d.IDPessoaNavigation)
                    .WithMany(p => p.Candidato)
                    .HasForeignKey(d => d.IDPessoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidato_Pessoa");

                entity.HasOne(d => d.IDPropostaNavigation)
                    .WithMany(p => p.Candidato)
                    .HasForeignKey(d => d.IDProposta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidato_Proposta");
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.Property(e => e.IDPessoa).ValueGeneratedNever();

                entity.Property(e => e.Bairro).IsUnicode(false);

                entity.Property(e => e.CEP).IsUnicode(false);

                entity.Property(e => e.Complemento).IsUnicode(false);

                entity.Property(e => e.Logradouro).IsUnicode(false);

                entity.Property(e => e.Municipio).IsUnicode(false);

                entity.Property(e => e.Numero).IsUnicode(false);

                entity.Property(e => e.Pais).IsUnicode(false);

                entity.Property(e => e.UF).IsUnicode(false);

                entity.HasOne(d => d.IDPessoaNavigation)
                    .WithOne(p => p.Endereco)
                    .HasForeignKey<Endereco>(d => d.IDPessoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Endereco_Pessoa");
            });

            modelBuilder.Entity<Imagem>(entity =>
            {
                entity.Property(e => e.IDPessoa).ValueGeneratedNever();

                entity.Property(e => e.Nome).IsUnicode(false);

                entity.HasOne(d => d.IDPessoaNavigation)
                    .WithOne(p => p.Imagem)
                    .HasForeignKey<Imagem>(d => d.IDPessoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Imagem_Pessoa");
            });

            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("IX_Pessoa")
                    .IsUnique();

                entity.Property(e => e.Celular).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Role).IsUnicode(false);

                entity.Property(e => e.Senha).IsUnicode(false);

                entity.Property(e => e.Telefone).IsUnicode(false);

                entity.Property(e => e.Tipo).IsUnicode(false);
            });

            modelBuilder.Entity<PessoaFisica>(entity =>
            {
                entity.HasIndex(e => e.CPF)
                    .HasName("IX_PessoaFisica")
                    .IsUnique();

                entity.Property(e => e.IDPessoa).ValueGeneratedNever();

                entity.Property(e => e.CPF).IsUnicode(false);

                entity.Property(e => e.Nome).IsUnicode(false);

                entity.Property(e => e.RG).IsUnicode(false);

                entity.HasOne(d => d.IDPessoaNavigation)
                    .WithOne(p => p.PessoaFisica)
                    .HasForeignKey<PessoaFisica>(d => d.IDPessoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PessoaFisica_Pessoa");
            });

            modelBuilder.Entity<PessoaJuridica>(entity =>
            {
                entity.HasIndex(e => e.CNPJ)
                    .HasName("IX_PessoaJuridica")
                    .IsUnique();

                entity.Property(e => e.IDPessoa).ValueGeneratedNever();

                entity.Property(e => e.CNPJ).IsUnicode(false);

                entity.Property(e => e.IE).IsUnicode(false);

                entity.Property(e => e.NomeFantasia).IsUnicode(false);

                entity.Property(e => e.RazaoSocial).IsUnicode(false);

                entity.HasOne(d => d.IDPessoaNavigation)
                    .WithOne(p => p.PessoaJuridica)
                    .HasForeignKey<PessoaJuridica>(d => d.IDPessoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PessoaJuridica_Pessoa");
            });

            modelBuilder.Entity<PessoaSkill>(entity =>
            {
                entity.HasKey(e => new { e.IDPessoa, e.IDSkill, e.IDTipoSkill });

                entity.HasOne(d => d.IDPessoaNavigation)
                    .WithMany(p => p.PessoaSkill)
                    .HasForeignKey(d => d.IDPessoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PessoaSkill_Pessoa");

                entity.HasOne(d => d.ID)
                    .WithMany(p => p.PessoaSkill)
                    .HasForeignKey(d => new { d.IDSkill, d.IDTipoSkill })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PessoaSkill_Skill");
            });

            modelBuilder.Entity<Proposta>(entity =>
            {
                entity.Property(e => e.IDProposta).ValueGeneratedNever();

                entity.Property(e => e.Cidade).IsUnicode(false);

                entity.Property(e => e.Descricao).IsUnicode(false);

                entity.Property(e => e.Latitude).IsUnicode(false);

                entity.Property(e => e.Longitude).IsUnicode(false);

                entity.Property(e => e.TipoContrato).IsUnicode(false);

                entity.HasOne(d => d.IDEmpresaNavigation)
                    .WithMany(p => p.Proposta)
                    .HasForeignKey(d => d.IDEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proposta_Pessoa1");
            });

            modelBuilder.Entity<PropostaSkill>(entity =>
            {
                entity.HasKey(e => new { e.IDProposta, e.IDSkill, e.IDTipoSkill });

                entity.HasOne(d => d.IDPropostaNavigation)
                    .WithMany(p => p.PropostaSkill)
                    .HasForeignKey(d => d.IDProposta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropostaSkill_Proposta");

                entity.HasOne(d => d.ID)
                    .WithMany(p => p.PropostaSkill)
                    .HasForeignKey(d => new { d.IDSkill, d.IDTipoSkill })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PropostaSkill_Skill");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => new { e.IDSkill, e.IDTipoSkill });

                entity.Property(e => e.IDSkill).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao).IsUnicode(false);

                entity.HasOne(d => d.IDTipoSkillNavigation)
                    .WithMany(p => p.Skill)
                    .HasForeignKey(d => d.IDTipoSkill)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Skill_TipoSkill");
            });

            modelBuilder.Entity<TipoSkill>(entity =>
            {
                entity.Property(e => e.IDTipoSkill).ValueGeneratedNever();

                entity.Property(e => e.Descricao).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}