using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class MixTape
    {
        public MixTape()
        {
            MixtapesCompositions = new HashSet<MixtapesCompositions>();
        }

        [Column("MixTapeID")]
        public Guid MixTapeId { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Сборник")]
        public string Title { get; set; }
        [Column(TypeName = "text")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [InverseProperty("MixTape")]
        public virtual ICollection<MixtapesCompositions> MixtapesCompositions { get; set; }
    }
}
