using MusicOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicOn.ViewModels
{
    public class CompositionViewModel
    {
        public CompositionViewModel()
        {
            IsFavourite = false;
        }
        public Composition Composition { get; set; }
        public bool IsFavourite { get; set; }
    }
}
