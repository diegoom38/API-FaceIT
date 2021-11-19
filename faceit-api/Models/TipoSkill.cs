using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace faceitapi.Models
{
    public partial class TipoSkill
    {
        public TipoSkill()
        {
            Skill = new HashSet<Skill>();
        }

        [Key]
        public int IDTipoSkill { get; set; }

        [Required]
        [StringLength(50)]
        public string Descricao { get; set; }

        [InverseProperty("IDTipoSkillNavigation")]
        public virtual ICollection<Skill> Skill { get; set; }
    }
}