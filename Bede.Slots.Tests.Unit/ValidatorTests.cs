using Bede.Slots.Core.Models;
using Bede.Slots.Core.Validators;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bede.Slots.Tests.Unit
{
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void Validate_BalanceGreaterThanZero()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 0
            };
            var expectedResult = false;
            var expectedMessage = "Your current balance is empty. Please deposit more funds to continue playing.";

            // Act
            var result = new SpinSlotsValidator().Validate(request);

            // Assert
            result.Success.Should().Be(expectedResult);
            result.FailureMessage.Should().Be(expectedMessage);
        }

        [TestMethod]
        public void Validate_StakeeGreaterThanZero()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 10,
                Stake = 0
            };
            var expectedResult = false;
            var expectedMessage = "Please stake a bet of 1 or higher.";

            // Act
            var result = new SpinSlotsValidator().Validate(request);

            // Assert
            result.Success.Should().Be(expectedResult);
            result.FailureMessage.Should().Be(expectedMessage);
        }

        [TestMethod]
        public void Validate_BalanceGreaterThanOrEqualStake()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 10,
                Stake = 11
            };
            var expectedResult = false;
            var expectedMessage = $"You can not stake more than your current balance. Please add a stake less than {request.Balance}.";

            // Act
            var result = new SpinSlotsValidator().Validate(request);

            // Assert
            result.Success.Should().Be(expectedResult);
            result.FailureMessage.Should().Be(expectedMessage);
        }

        [TestMethod]
        public void Validate_ValidSpinSlotsRequest()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 10,
                Stake = 10
            };
            var expectedResult = true;

            // Act
            var result = new SpinSlotsValidator().Validate(request);

            // Assert
            result.Success.Should().Be(expectedResult);
            result.FailureMessage.Should().BeNullOrWhiteSpace();
        }
    }
}
