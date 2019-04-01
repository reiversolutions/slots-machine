using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Models
{
    public class SpinSlotsResponse
    {
        public decimal Balance { get; set; }
        public decimal Winnings { get; set; }
        public string[] Rows { get; set; }
    }
}
