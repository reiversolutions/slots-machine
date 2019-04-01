using System;
using System.Collections.Generic;
using System.Text;
using Bede.Slots.Core.Models;

namespace Bede.Slots.Core.Validators
{
    public class SpinSlotsValidator : IValidator<SpinSlotsRequest, bool>
    {
        public Result<bool> Validate(SpinSlotsRequest request)
        {
            if (request.Balance <= 0)
                return Result<bool>.OnFailure("Your current balance is empty. Please deposit more funds to continue playing.");

            if (request.Stake <= 0)
                return Result<bool>.OnFailure("Please stake a bet of 1 or higher.");

            if (request.Stake > request.Balance)
                return Result<bool>.OnFailure($"You can not stake more than your current balance. Please add a stake less than {request.Balance}.");

            return Result<bool>.OnSuccess(true);
        }
    }
}
