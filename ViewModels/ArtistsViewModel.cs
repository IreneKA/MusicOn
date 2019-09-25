using MusicOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicOn.ViewModels
{
    public class ArtistsViewModel
    {
        public ArtistsViewModel()
        {
            IsFavourite = false;
            //Compositions = new List<CompositionViewModel>();
            Albums = new List<Album>();
        }
        public Artist Artist { get; set; }
        public bool IsFavourite { get; set; }
        //public List<CompositionViewModel> Compositions { get; set; }
        public List<Album> Albums { get; set; }
    }
}
