using MusicOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicOn.ViewModels
{
    public class MixTapeViewModel
    {
        public MixTapeViewModel()
        {
            Compositions = new List<CompositionViewModel>();
        }
        public MixTape MixTape { get; set; }
        public List<CompositionViewModel> Compositions { get; set; }
    }
}
