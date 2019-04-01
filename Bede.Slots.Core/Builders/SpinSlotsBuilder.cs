using Bede.Slots.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Builders
{
    public interface ISpinSlotsBuilder
    {
        Result<SpinSlotsResponse> BuildSpin(SpinSlotsRequest request);
    }

    public class SpinSlotsBuilder : ISpinSlotsBuilder
    {
        public static int Rows => 4;

        private IRowBuilder RowBuilder { get; }
        private IOutcomeBuilder OutcomeBuilder { get; }
        private ISpinSlotsResponseBuilder ResponseBuilder { get; }

        public SpinSlotsBuilder(IRowBuilder rowBuilder, IOutcomeBuilder outcomeBuilder, ISpinSlotsResponseBuilder spinSlotsResponseBuilder)
        {
            RowBuilder = rowBuilder ?? throw new ArgumentNullException(nameof(rowBuilder));
            OutcomeBuilder = outcomeBuilder ?? throw new ArgumentNullException(nameof(outcomeBuilder));
            ResponseBuilder = spinSlotsResponseBuilder ?? throw new ArgumentNullException(nameof(spinSlotsResponseBuilder));
        }

        public Result<SpinSlotsResponse> BuildSpin(SpinSlotsRequest request)
        {
            var totalOutcome = 0m;
            var rows = new List<SymbolBase>();
            for (var i = 0; i < Rows; i++)
            {
                var rowResult = RowBuilder.BuildRow();
                if (!rowResult.Success)
                    return Result<SpinSlotsResponse>.OnFailure(rowResult.FailureMessage);

                rows.AddRange(rowResult.Data);

                var outcomeResult = OutcomeBuilder.Build(rowResult.Data);
                if (!outcomeResult.Success)
                    return Result<SpinSlotsResponse>.OnFailure(rowResult.FailureMessage);

                totalOutcome += outcomeResult.Data;
            }

            return ResponseBuilder.Build(request, rows, totalOutcome);
        }
    }
}
