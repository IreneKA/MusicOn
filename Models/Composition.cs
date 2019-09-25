using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicOn.Models
{
    public partial class Composition
    {
        public Composition()
        {
            FavouriteSongs = new HashSet<FavouriteSongs>();
            LoadedSongs = new HashSet<LoadedSongs>();
            MixtapesCompositions = new HashSet<MixtapesCompositions>();
        }

        [Column("CompositionID")]
        public Guid CompositionId { get; set; }
        [Column("SongID")]
        [Display(Name = "Название")]
        public Guid SongId { get; set; }
        [Column("AlbumID")]
        [Display(Name = "Альбом")]
        public Guid AlbumId { get; set; }
        [Column(TypeName = "time(0)")]
        [Display(Name = "Длительность")]
        public TimeSpan Duration { get; set; }

        [ForeignKey("AlbumId")]
        [InverseProperty("Composition")]
        [Display(Name = "Альбом")]
        public virtual Album Album { get; set; }
        [ForeignKey("SongId")]
        [InverseProperty("Composition")]
        [Display(Name ="Название")]
        public virtual Song Song { get; set; }
        [InverseProperty("Composition")]
        public virtual ICollection<FavouriteSongs> FavouriteSongs { get; set; }
        [InverseProperty("Composition")]
        public virtual ICollection<LoadedSongs> LoadedSongs { get; set; }
        [InverseProperty("Composition")]
        public virtual ICollection<MixtapesCompositions> MixtapesCompositions { get; set; }
    }
}
