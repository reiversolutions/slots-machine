using Bede.Slots.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bede.Slots.Core.Builders
{
    public interface IOutcomeBuilder
    {
        Result<decimal> Build(List<SymbolBase> row);
    }

    public class OutcomeBuilder : IOutcomeBuilder
    {
        public Result<decimal> Build(List<SymbolBase> row)
        {
            var winningSet = GetWinningSet(row);

            if (winningSet == null)
                return Result<decimal>.OnSuccess(0);

            var totalCoefficient = CalculateTotalCoefficient(winningSet);

            return Result<decimal>.OnSuccess(totalCoefficient);
        }

        public List<SymbolBase> GetWinningSet(List<SymbolBase> row)
        {
            var groups = row.GroupBy(symbol => symbol.Symbol)
                            .Select(g => g.ToList())
                            .ToList();
            var wildcards = groups.SingleOrDefault(g => g.FirstOrDefault() is Wildcard)?.Count() ?? 0;

            return groups.Where(g => g.Count() == RowBuilder.Columns - wildcards)
                            .OrderBy(g => g.FirstOrDefault()?.Coefficient ?? 0)
                            .FirstOrDefault();
        }

        public decimal CalculateTotalCoefficient(List<SymbolBase> winningSet)
        {
            var totalCoefficient = 0m;
            foreach (var symbol in winningSet)
            {
                totalCoefficient += symbol.Coefficient;
            }

            return totalCoefficient;
        }
    }
}
