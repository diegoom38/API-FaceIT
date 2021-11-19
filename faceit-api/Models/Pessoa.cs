using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class Pessoa
    {
        public Pessoa()
        {
            Candidato = new HashSet<Candidato>();
            PessoaSkill = new HashSet<PessoaSkill>();
            Proposta = new HashSet<Proposta>();
        }

        [Key]
        public int IDPessoa { get; set; }

        [StringLength(2)]
        public string Tipo { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(150)]
        public string Senha { get; set; }

        public bool Excluido { get; set; }
        public int? GoogleID { get; set; }

        [StringLength(15)]
        public string Celular { get; set; }

        [StringLength(14)]
        public string Telefone { get; set; }

        [StringLength(10)]
        public string Role { get; set; }

        [InverseProperty("IDPessoaNavigation")]
        public virtual Anexo Anexo { get; set; }

        [InverseProperty("IDPessoaNavigation")]
        public virtual Endereco Endereco { get; set; }

        [InverseProperty("IDPessoaNavigation")]
        public virtual Imagem Imagem { get; set; }

        [InverseProperty("IDPessoaNavigation")]
        public virtual PessoaFisica PessoaFisica { get; set; }

        [InverseProperty("IDPessoaNavigation")]
        public virtual PessoaJuridica PessoaJuridica { get; set; }

        [InverseProperty("IDPessoaNavigation")]
        public virtual ICollection<Candidato> Candidato { get; set; }

        [InverseProperty("IDPessoaNavigation")]
        public virtual ICollection<PessoaSkill> PessoaSkill { get; set; }

        [InverseProperty("IDEmpresaNavigation")]
        public virtual ICollection<Proposta> Proposta { get; set; }
    }
}