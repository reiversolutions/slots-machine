using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Models
{
    public class Apple : SymbolBase
    {
        public override string DisplayName => "Apple";
        public override string Symbol => "A";
        public override decimal Coefficient => 0.4m;
        public override int Probability => 45;
    }
}
