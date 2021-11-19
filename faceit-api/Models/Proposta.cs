using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class Proposta
    {
        public Proposta()
        {
            Candidato = new HashSet<Candidato>();
            PropostaSkill = new HashSet<PropostaSkill>();
        }

        [Key]
        public int IDProposta { get; set; }

        public int IDEmpresa { get; set; }

        [StringLength(250)]
        public string Descricao { get; set; }

        [StringLength(2)]
        public string TipoContrato { get; set; }

        [StringLength(150)]
        public string Cidade { get; set; }

        public bool? Encerrada { get; set; }

        [StringLength(150)]
        public string Latitude { get; set; }

        [StringLength(150)]
        public string Longitude { get; set; }

        [ForeignKey(nameof(IDEmpresa))]
        [InverseProperty(nameof(Pessoa.Proposta))]
        public virtual Pessoa IDEmpresaNavigation { get; set; }

        [InverseProperty("IDPropostaNavigation")]
        public virtual ICollection<Candidato> Candidato { get; set; }

        [InverseProperty("IDPropostaNavigation")]
        public virtual ICollection<PropostaSkill> PropostaSkill { get; set; }
    }
}