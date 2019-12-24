using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class LoadedSongs
    {
        [Column("UserID")]
        public Guid UserId { get; set; }
        [Column("CompositionID")]
        public Guid CompositionId { get; set; }
        [Column(TypeName = "datetime")]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [ForeignKey("CompositionId")]
        [InverseProperty("LoadedSongs")]
        [Display(Name ="Композиция")]
        public virtual Composition Composition { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("LoadedSongs")]
        public virtual User User { get; set; }
    }
}
