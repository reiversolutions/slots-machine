using Bede.Slots.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Core.Validators
{
    public interface IValidator<T, R> where T : class
    {
        Result<R> Validate(T objectToValidate);
    }
}
