using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicOn.Models;

namespace MusicOn.ViewModels
{
    public class SearchModel
    {
        public SearchModel()
        {
            Compositions = new List<CompositionViewModel>();
            Artists = new List<ArtistsViewModel>();
            Albums = new List<Album>();
            MixTapes = new List<MixTape>();
        }
        public List<CompositionViewModel> Compositions { get; set; }
        public List<ArtistsViewModel> Artists { get; set; }
        public List<Album> Albums { get; set; }
        public List<MixTape> MixTapes { get; set; }
    }
}
