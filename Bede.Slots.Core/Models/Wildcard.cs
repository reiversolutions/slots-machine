using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Models
{
    public class Wildcard : SymbolBase
    {
        public override string DisplayName => "Wildcard";
        public override string Symbol => "*";
        public override decimal Coefficient => 0;
        public override int Probability => 5;
    }
}
