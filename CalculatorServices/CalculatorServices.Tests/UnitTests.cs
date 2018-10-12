using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CalculatorServices.Data;
using CalculatorServices.Data.Core;
using CalculatorServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CalculatorServices.Tests
{
    [TestClass]
    public class UnitTests
    {

        [TestMethod]
        public async Task AdditionService_Add_Should_Pass()
        {
            var historyServiceMock = new Mock<IHistoryClient>();

            var repositoryMock = new Mock<IAuditRepository<AdditionAuditRecord>>();
            var mockDbSet = new Mock<DbSet<AdditionAuditRecord>>();
            repositoryMock.Setup(c => c.Audits).Returns(mockDbSet.Object);

            var value = 5;
            historyServiceMock
                .Setup(c => c.UpdateHistory(It.IsAny<Guid>(), Operations.Addition, value, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() { CalculatorId = Guid.NewGuid(), NewValue = 5, OldValue = null, Operation = Operations.Addition });

            var service = new AdditionService(repositoryMock.Object, historyServiceMock.Object);
            var result = await service.Add(null, value, CancellationToken.None);

            repositoryMock.Verify(r => r.AddAudit(It.IsAny<AdditionAuditRecord>(), CancellationToken.None), Times.Once);
            historyServiceMock.Verify(c => c.GetLast(It.IsAny<Guid>(), CancellationToken.None), Times.Never);

            Assert.AreEqual(value, result.Result);
            Assert.AreNotEqual(result.GlobalId, default(Guid));

        }

        [TestMethod]
        public async Task AdditionService_Add_Existing_Id_Should_Pass()
        {
            var repositoryMock = new Mock<IAuditRepository<AdditionAuditRecord>>();
            var historyServiceMock = new Mock<IHistoryClient>();

            var mockDbSet = new Mock<DbSet<AdditionAuditRecord>>();
            repositoryMock.Setup(c => c.Audits).Returns(mockDbSet.Object);

            var id = Guid.NewGuid();
            var value = 5;
            var expectedOutput = 15;

            historyServiceMock
                .Setup(c => c.GetLast(id, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() {CalculatorId = id, OldValue = 0, NewValue = 10, Operation = Operations.Addition});

            historyServiceMock
                .Setup(c => c.UpdateHistory(id, Operations.Addition, expectedOutput, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() { CalculatorId = id, NewValue = expectedOutput, OldValue = 10, Operation = Operations.Addition });


            var service = new AdditionService(repositoryMock.Object, historyServiceMock.Object);
            var result = await service.Add(id, value, CancellationToken.None);

            repositoryMock.Verify(r => r.AddAudit(It.IsAny<AdditionAuditRecord>(), CancellationToken.None), Times.Once);
            historyServiceMock.Verify(c => c.GetLast(It.IsAny<Guid>(), CancellationToken.None), Times.Once);

            Assert.AreEqual(expectedOutput, result.Result);
            Assert.AreEqual(id, result.GlobalId);

        }

        [TestMethod]
        public async Task SubtractionService_Subtract_Should_Pass()
        {
            var repositoryMock = new Mock<IAuditRepository<SubtractionAuditRecord>>();
            var historyServiceMock = new Mock<IHistoryClient>();

            var mockDbSet = new Mock<DbSet<SubtractionAuditRecord>>();
            repositoryMock.Setup(c => c.Audits).Returns(mockDbSet.Object);

            var value = 5;
            historyServiceMock
                .Setup(c => c.UpdateHistory(It.IsAny<Guid>(), Operations.Subtraction, -value, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() { CalculatorId = Guid.NewGuid(), NewValue = -value, OldValue = null, Operation = Operations.Subtraction });

            var service = new SubtractionService(repositoryMock.Object, historyServiceMock.Object);
            var result = await service.Subtract(null, value, CancellationToken.None);

            repositoryMock.Verify(r => r.AddAudit(It.IsAny<SubtractionAuditRecord>(), CancellationToken.None), Times.Once);
            historyServiceMock.Verify(c => c.GetLast(It.IsAny<Guid>(), CancellationToken.None), Times.Never);

            Assert.AreEqual(-value, result.Result);
            Assert.AreNotEqual(result.GlobalId, default(Guid));

        }

        [TestMethod]
        public async Task SubtractionService_Subtract_Existing_Id_Should_Pass()
        {
            var repositoryMock = new Mock<IAuditRepository<SubtractionAuditRecord>>();
            var historyServiceMock = new Mock<IHistoryClient>();

            var mockDbSet = new Mock<DbSet<SubtractionAuditRecord>>();
            repositoryMock.Setup(c => c.Audits).Returns(mockDbSet.Object);

            var id = Guid.NewGuid();
            var value = 5;
            var expectedOutput = 5;

            historyServiceMock
                .Setup(c => c.GetLast(id, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() {CalculatorId = id, OldValue = 0, NewValue = 10, Operation = Operations.Subtraction});

            historyServiceMock
                .Setup(c => c.UpdateHistory(id, Operations.Subtraction, expectedOutput, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() { CalculatorId = id, NewValue = expectedOutput, OldValue = 10, Operation = Operations.Subtraction });


            var service = new SubtractionService(repositoryMock.Object, historyServiceMock.Object);
            var result = await service.Subtract(id, value, CancellationToken.None);

            repositoryMock.Verify(r => r.AddAudit(It.IsAny<SubtractionAuditRecord>(), CancellationToken.None), Times.Once);
            historyServiceMock.Verify(c => c.GetLast(It.IsAny<Guid>(), CancellationToken.None), Times.Once);

            Assert.AreEqual(expectedOutput, result.Result);
            Assert.AreEqual(id, result.GlobalId);
        }
    }
}
