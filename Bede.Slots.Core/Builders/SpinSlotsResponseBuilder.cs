using Bede.Slots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bede.Slots.Core.Builders
{
    public interface ISpinSlotsResponseBuilder
    {
        Result<SpinSlotsResponse> Build(SpinSlotsRequest request, List<SymbolBase> rows, decimal outcome);
    }

    public class SpinSlotsResponseBuilder : ISpinSlotsResponseBuilder
    {
        public Result<SpinSlotsResponse> Build(SpinSlotsRequest request, List<SymbolBase> rows, decimal outcome)
        {
            var currentBalance = request.Balance - request.Stake;
            var winnings = outcome * request.Stake;
            currentBalance += winnings;

            return Result<SpinSlotsResponse>.OnSuccess(new SpinSlotsResponse()
            {
                Balance = Math.Max(0, Math.Round(currentBalance, 2)),
                Winnings = Math.Round(winnings, 2),
                Rows = rows.Select(symbol => symbol.Symbol).ToArray<string>()
            });
        }
    }
}
