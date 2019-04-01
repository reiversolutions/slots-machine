using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Models
{
    public abstract class SymbolBase
    {
        public abstract string DisplayName { get; }
        public abstract string Symbol { get; }
        public abstract decimal Coefficient { get; }
        public abstract int Probability { get;  }
    }
}
