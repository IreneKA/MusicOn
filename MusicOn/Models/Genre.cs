using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Album = new HashSet<Album>();
        }

        [Column("GenreID")]
        public int GenreId { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [InverseProperty("Genre")]
        public virtual ICollection<Album> Album { get; set; }
    }
}
