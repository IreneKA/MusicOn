using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class Album
    {
        public Album()
        {
            AlbumsArtists = new HashSet<AlbumsArtists>();
            Composition = new HashSet<Composition>();
        }

        [Column("AlbumID")]
        public Guid AlbumId { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата выпуска")]
        public DateTime Date { get; set; }
        [Display(Name = "Сингл")]
        public bool Single { get; set; }
        [Column("GenreID")]
        [Display(Name = "Жанр")]
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        [Display(Name = "Жанр")]
        [InverseProperty("Album")]
        public virtual Genre Genre { get; set; }
        [InverseProperty("Album")]
        [Display(Name = "Исполнители")]
        public virtual ICollection<AlbumsArtists> AlbumsArtists { get; set; }
        [Display(Name = "Композиции")]
        [InverseProperty("Album")]
        public virtual ICollection<Composition> Composition { get; set; }
    }
}
