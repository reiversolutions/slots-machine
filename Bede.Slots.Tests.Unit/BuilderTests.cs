using Bede.Slots.Core.Builders;
using Bede.Slots.Core.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bede.Slots.Tests.Unit
{
    [TestClass]
    public class BuilderTests
    {
        public ISpinSlotsBuilder Builder { get; set; }

        private Mock<IRowBuilder> _mockRowBuilder;
        private Mock<IOutcomeBuilder> _mockOutcomeBuilder;
        private Mock<ISpinSlotsResponseBuilder> _mockSpinSlotsResponseBuilder;

        [TestInitialize]
        public void SetUp()
        {
            _mockRowBuilder = new Mock<IRowBuilder>();
            _mockRowBuilder.Setup(mrb => mrb.BuildRow()).Returns(new Result<List<SymbolBase>>()
            {
                Success = true,
                FailureMessage = string.Empty,
                Data = new List<SymbolBase>()
                {
                    new Apple(),
                    new Apple(),
                    new Apple()
                }
            });

            _mockOutcomeBuilder = new Mock<IOutcomeBuilder>();
            _mockOutcomeBuilder.Setup(mob => mob.Build(It.IsAny<List<SymbolBase>>()))
                                .Returns((List<SymbolBase> row) => new OutcomeBuilder().Build(row));

            _mockSpinSlotsResponseBuilder = new Mock<ISpinSlotsResponseBuilder>();
            _mockSpinSlotsResponseBuilder.Setup(
                                                mssrb =>
                                                    mssrb.Build(It.IsAny<SpinSlotsRequest>(), It.IsAny<List<SymbolBase>>(), It.IsAny<decimal>()))
                                            .Returns(
                                                (SpinSlotsRequest request, List<SymbolBase> rows, decimal outcome) =>
                                                    new SpinSlotsResponseBuilder().Build(request, rows, outcome));

            Builder = new SpinSlotsBuilder(_mockRowBuilder.Object, _mockOutcomeBuilder.Object, _mockSpinSlotsResponseBuilder.Object);
        }

        [TestMethod]
        public void BuildRow_RowShouldHaveXColumns()
        {
            // Assign
            var expectedNoOfColumns = 3;

            // Act
            var result = new RowBuilder().BuildRow();

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Count.Should().Be(expectedNoOfColumns);
        }

        [TestMethod]
        public void HandleProbablity_Apple()
        {
            // Assign
            var probability = 0;

            // Act
            var result = new RowBuilder().HandleProbablity(probability);

            // Assert
            (result is Apple).Should().BeTrue();
        }

        [TestMethod]
        public void HandleProbablity_Banana()
        {
            // Assign
            var probability = 46;

            // Act
            var result = new RowBuilder().HandleProbablity(probability);

            // Assert
            (result is Banana).Should().BeTrue();
        }

        [TestMethod]
        public void HandleProbablity_Pineapple()
        {
            // Assign
            var probability = 81;

            // Act
            var result = new RowBuilder().HandleProbablity(probability);

            // Assert
            (result is Pineapple).Should().BeTrue();
        }

        [TestMethod]
        public void HandleProbablity_Wildcard()
        {
            // Assign
            var probability = 96;

            // Act
            var result = new RowBuilder().HandleProbablity(probability);

            // Assert
            (result is Wildcard).Should().BeTrue();
        }

        [TestMethod]
        public void HandleProbablity_ThrowException()
        {
            // Assign
            var probability = 101;

            // Act
            Action action = () => { new RowBuilder().HandleProbablity(probability); };
            
            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void BuildOutcome_NoPattern()
        {
            // Assign
            var row = new List<SymbolBase>()
            {
                new Wildcard(),
                new Apple(),
                new Banana()
            };
            var expectedOutcome = 0m;

            // Act
            var result = new OutcomeBuilder().Build(row);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().Be(expectedOutcome);
        }

        [TestMethod]
        public void BuildOutcome_PatternNoWildCards()
        {
            // Assign
            var row = new List<SymbolBase>()
            {
                new Apple(),
                new Apple(),
                new Apple()
            };
            var expectedOutcome = 1.2m;

            // Act
            var result = new OutcomeBuilder().Build(row);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().Be(expectedOutcome);
        }

        [TestMethod]
        public void BuildOutcome_PatternSingleWildCard()
        {
            // Assign
            var row = new List<SymbolBase>()
            {
                new Apple(),
                new Wildcard(),
                new Apple()
            };
            var expectedOutcome = 0.8m;

            // Act
            var result = new OutcomeBuilder().Build(row);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().Be(expectedOutcome);
        }

        [TestMethod]
        public void BuildOutcome_PatternMultipleWildCards()
        {
            // Assign
            var row = new List<SymbolBase>()
            {
                new Apple(),
                new Wildcard(),
                new Wildcard()
            };
            var expectedOutcome = 0.4m;

            // Act
            var result = new OutcomeBuilder().Build(row);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().Be(expectedOutcome);
        }

        [TestMethod]
        public void BuildOutcome_AllWildCards()
        {
            // Assign
            var row = new List<SymbolBase>()
            {
                new Wildcard(),
                new Wildcard(),
                new Wildcard()
            };
            var expectedOutcome = 0m;

            // Act
            var result = new OutcomeBuilder().Build(row);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Should().Be(expectedOutcome);
        }

        [TestMethod]
        public void BuildResponse_StakeCostRemoved()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 200,
                Stake = 10
            };
            var rows = new List<SymbolBase>();
            var outcome = 0;
            var expectedBalance = 190;

            // Act
            var result = new SpinSlotsResponseBuilder().Build(request, rows, outcome);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Balance.Should().Be(expectedBalance);
        }

        [TestMethod]
        public void BuildResponse_OutcomeCostAppended()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 200,
                Stake = 1
            };
            var rows = new List<SymbolBase>();
            var outcome = 10;
            var expectedBalance = 209;

            // Act
            var result = new SpinSlotsResponseBuilder().Build(request, rows, outcome);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Balance.Should().Be(expectedBalance);
        }

        [TestMethod]
        public void BuildResponse_BalanceUpdated()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 200,
                Stake = 10
            };
            var rows = new List<SymbolBase>();
            var outcome = 2;
            var expectedBalance = 210;

            // Act
            var result = new SpinSlotsResponseBuilder().Build(request, rows, outcome);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Balance.Should().Be(expectedBalance);
        }

        [TestMethod]
        public void BuildResponse_DisplayNameReturned()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 200,
                Stake = 10
            };
            var rows = new List<SymbolBase>()
            {
                new Apple()
            };
            var outcome = 2;

            // Act
            var result = new SpinSlotsResponseBuilder().Build(request, rows, outcome);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Rows.All(item => item == new Apple().Symbol).Should().BeTrue();
        }

        [TestMethod]
        public void BuildResponse_BalancePositiveOrZero()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 5,
                Stake = 10
            };
            var rows = new List<SymbolBase>();
            var outcome = 0;

            // Act
            var result = new SpinSlotsResponseBuilder().Build(request, rows, outcome);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Balance.Should().Be(0);
        }

        [TestMethod]
        public void BuildSpin_WinningSpin()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 200,
                Stake = 10
            };
            var expectedBalance = 238;

            // Act
            var result = Builder.BuildSpin(request);

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Balance.Should().Be(expectedBalance);
        }

        [TestMethod]
        public void BuildSpin_VerifyBuilders()
        {
            // Assign
            var request = new SpinSlotsRequest()
            {
                Balance = 200,
                Stake = 10
            };
            
            // Act
            var result = Builder.BuildSpin(request);

            // Assert
            _mockRowBuilder.Verify(mrb => mrb.BuildRow(), Times.Exactly(SpinSlotsBuilder.Rows));
            _mockOutcomeBuilder.Verify(mob => mob.Build(It.IsAny<List<SymbolBase>>()), Times.Exactly(SpinSlotsBuilder.Rows));
            _mockSpinSlotsResponseBuilder.Verify(mob => mob.Build(It.IsAny<SpinSlotsRequest>(), It.IsAny<List<SymbolBase>>(), It.IsAny<decimal>()), Times.Once);
        }
    }
}
