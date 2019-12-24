using MusicOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicOn.ViewModels
{
    public class AlbumViewModel
    {
        public AlbumViewModel()
        {
            Compositions = new List<CompositionViewModel>();
        }
        public Album Album { get; set; }
        public List<CompositionViewModel> Compositions { get; set; }
    }
}
