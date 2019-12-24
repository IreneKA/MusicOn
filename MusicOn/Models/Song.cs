using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class Song
    {
        public Song()
        {
            Composition = new HashSet<Composition>();
        }

        [Column("SongID")]
        public Guid SongId { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [InverseProperty("Song")]
        public virtual ICollection<Composition> Composition { get; set; }
    }
}
