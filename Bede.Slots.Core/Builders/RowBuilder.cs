using Bede.Slots.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Builders
{
    public interface IRowBuilder
    {
        Result<List<SymbolBase>> BuildRow();
    }

    public class RowBuilder : IRowBuilder
    {
        public static int Columns => 3;

        public Result<List<SymbolBase>> BuildRow()
        {
            var row = new List<SymbolBase>();
            for (var i = 0; i < Columns; i++)
            {
                try
                {
                    row.Add(BuildColumn());
                } catch (ArgumentOutOfRangeException)
                {
                    return Result<List<SymbolBase>>.OnFailure("There was an error creating the slots row.");
                }
            }

            return Result<List<SymbolBase>>.OnSuccess(row);
        }

        public SymbolBase BuildColumn()
        {
            var random = new Random();
            var probability = random.Next(0, 100);

            return HandleProbablity(probability);
        }

        public SymbolBase HandleProbablity(int probability)
        {
            var appleBoundary = new Apple().Probability;
            var bananaBoundary = appleBoundary + new Banana().Probability;
            var pineappleBoundary = bananaBoundary + new Pineapple().Probability;
            var wildCardBoundary = pineappleBoundary + new Wildcard().Probability;

            if (probability >= 0 && probability <= appleBoundary)
                return new Apple();

            if (probability > appleBoundary && probability <= bananaBoundary)
                return new Banana();

            if (probability > bananaBoundary && probability <= pineappleBoundary)
                return new Pineapple();

            if (probability > pineappleBoundary && probability <= wildCardBoundary)
                return new Wildcard();

            throw new ArgumentOutOfRangeException($"No defined symbol for probability {probability}.");
        }
    }
}
