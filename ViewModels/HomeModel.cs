using MusicOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicOn.ViewModels
{
    public class HomeModel
    {
        public HomeModel()
        {
            Compositions = new List<CompositionViewModel>();
            Albums = new List<Album>();
        }
        public List<CompositionViewModel> Compositions { get; set; }
        public List<Album> Albums { get; set; }
    }
}
