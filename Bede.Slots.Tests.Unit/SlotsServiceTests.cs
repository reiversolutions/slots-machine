using Bede.Slots.Core.Builders;
using Bede.Slots.Core.Models;
using Bede.Slots.Core.Services;
using Bede.Slots.Core.Validators;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.Slots.Tests.Unit
{
    [TestClass]
    public class SlotsServiceTests
    {
        public ISlotsService Service { get; set; }

        private Mock<IValidator<SpinSlotsRequest, bool>> _mockValidator;
        private Mock<ISpinSlotsBuilder> _mockBuilder;

        [TestInitialize]
        public void SetUp()
        {
            _mockValidator = new Mock<IValidator<SpinSlotsRequest, bool>>();
            _mockValidator.Setup(mv => mv.Validate(It.Is<SpinSlotsRequest>(r => r.Balance >= r.Stake))).Returns(Result<bool>.OnSuccess(true));
            _mockValidator.Setup(mv => mv.Validate(It.Is<SpinSlotsRequest>(r => r.Balance < r.Stake))).Returns(Result<bool>.OnFailure("Validation failed"));

            _mockBuilder = new Mock<ISpinSlotsBuilder>();
            _mockBuilder.Setup(
                            mb => mb.BuildSpin(It.IsAny<SpinSlotsRequest>()))
                        .Returns(
                            (SpinSlotsRequest request) => 
                                new SpinSlotsBuilder(new RowBuilder(), new OutcomeBuilder(), new SpinSlotsResponseBuilder()).BuildSpin(request));

            Service = new SlotsService(_mockValidator.Object, _mockBuilder.Object);
        }

        [TestMethod]
        public void Spin_ValidatorHasRan()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 10,
                Stake = 5
            };

            // Act
            var result = Service.Spin(request);

            // Assert
            _mockValidator.Verify(mv => mv.Validate(request), Times.Once);
        }

        [TestMethod]
        public void Spin_ValidatorHasFailed()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 10,
                Stake = 20
            };

            // Act
            var result = Service.Spin(request);

            // Assert
            _mockValidator.Verify(mv => mv.Validate(request), Times.Once);
            _mockBuilder.Verify(mb => mb.BuildSpin(request), Times.Never);
        }

        [TestMethod]
        public void Spin_BuilderHasRan()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 10,
                Stake = 5
            };

            // Act
            var result = Service.Spin(request);

            // Assert
            _mockBuilder.Verify(mb => mb.BuildSpin(request), Times.Once);
        }

        [TestMethod]
        public void Spin_ValidSpin()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 10,
                Stake = 5
            };

            // Act
            var result = Service.Spin(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Balance.Should().BeGreaterOrEqualTo(5);
        }
    }
}
