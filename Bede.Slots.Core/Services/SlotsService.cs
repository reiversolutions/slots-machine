using Bede.Slots.Core.Builders;
using Bede.Slots.Core.Models;
using Bede.Slots.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Services
{
    public interface ISlotsService
    {
        Result<SpinSlotsResponse> Spin(SpinSlotsRequest request);
    }

    public class SlotsService : ISlotsService
    {
        private IValidator<SpinSlotsRequest, bool> Validator { get; }
        private ISpinSlotsBuilder Builder { get; }

        public SlotsService(IValidator<SpinSlotsRequest, bool> validator, ISpinSlotsBuilder spinSlotsBuilder)
        {
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            Builder = spinSlotsBuilder ?? throw new ArgumentNullException(nameof(spinSlotsBuilder));
        }

        public Result<SpinSlotsResponse> Spin(SpinSlotsRequest request)
        {
            var result = Validator.Validate(request);
            if (!result.Success)
                return Result<SpinSlotsResponse>.OnFailure(result.FailureMessage);
            
            return Builder.BuildSpin(request);
        }
    }
}
