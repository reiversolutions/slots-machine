using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Models
{
    public class Pineapple : SymbolBase
    {
        public override string DisplayName => "Pineapple";
        public override string Symbol => "P";
        public override decimal Coefficient => 0.8m;
        public override int Probability => 15;
    }
}
