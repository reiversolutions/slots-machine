using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Models
{
    public class Banana : SymbolBase
    {
        public override string DisplayName => "Banana";
        public override string Symbol => "B";
        public override decimal Coefficient => 0.6m;
        public override int Probability => 35;
    }
}
