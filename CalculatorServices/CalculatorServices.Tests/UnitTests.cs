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
            var calculatorRepositoryMock = new Mock<ICalculatorRepository>();
            var calculatorSetMock = new Mock<DbSet<CalculatorRecord>>();
            var auditRepositoryMock = new Mock<IAuditRepository<AdditionAuditRecord>>();
            var auditSetMock = new Mock<DbSet<AdditionAuditRecord>>();

            calculatorRepositoryMock.Setup(c => c.Calculators).Returns(calculatorSetMock.Object);
            auditRepositoryMock.Setup(c => c.Audits).Returns(auditSetMock.Object);

            var value = 5M;
            var operation = Operations.Addition;
            var expectedResult = value;

            historyServiceMock
                .Setup(c => c.UpdateHistory(It.IsAny<Guid>(), operation, value, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() { CalculatorId = Guid.NewGuid(), NewValue = expectedResult, OldValue = null, Operation = Operations.Addition });

            var service = new AdditionService(calculatorRepositoryMock.Object, auditRepositoryMock.Object, historyServiceMock.Object);
            var result = await service.Add(null, value, CancellationToken.None);

            Assert.AreEqual(expectedResult, result.Result);
            Assert.AreNotEqual(result.GlobalId, default(Guid));

            calculatorRepositoryMock.Verify(r => r.Add(It.IsAny<CalculatorRecord>(), CancellationToken.None), Times.Once);
            auditRepositoryMock.Verify(r => r.AddAudit(It.IsAny<AdditionAuditRecord>(), CancellationToken.None), Times.Once);
            historyServiceMock.Verify(c => c.UpdateHistory(It.IsAny<Guid>(), operation, expectedResult, CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task AdditionService_Add_Existing_Id_Should_Pass()
        {
            var historyServiceMock = new Mock<IHistoryClient>();
            var calculatorRepositoryMock = new Mock<ICalculatorRepository>();
            var calculatorSetMock = new Mock<DbSet<CalculatorRecord>>();
            var auditRepositoryMock = new Mock<IAuditRepository<AdditionAuditRecord>>();
            var auditSetMock = new Mock<DbSet<AdditionAuditRecord>>();

            var id = Guid.NewGuid();
            var value = 10M;
            var operand = 5M;
            var operation = Operations.Addition;
            var expectedOutput = value + operand;

            calculatorRepositoryMock.Setup(c => c.Calculators).Returns(calculatorSetMock.Object);
            auditRepositoryMock.Setup(c => c.Audits).Returns(auditSetMock.Object);
            calculatorRepositoryMock
                .Setup(c => c.Find(id, CancellationToken.None))
                .ReturnsAsync(new CalculatorRecord { Id = id, Value = value });
            historyServiceMock
                .Setup(c => c.UpdateHistory(id, operation, expectedOutput, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() { CalculatorId = id, NewValue = expectedOutput, OldValue = value, Operation = Operations.Addition });

            var service = new AdditionService(calculatorRepositoryMock.Object, auditRepositoryMock.Object, historyServiceMock.Object);
            var result = await service.Add(id, operand, CancellationToken.None);

            Assert.AreEqual(expectedOutput, result.Result);
            Assert.AreEqual(id, result.GlobalId);

            calculatorRepositoryMock.Verify(r => r.Update(It.IsAny<CalculatorRecord>(), CancellationToken.None), Times.Once);
            auditRepositoryMock.Verify(r => r.AddAudit(It.IsAny<AdditionAuditRecord>(), CancellationToken.None), Times.Once);
            historyServiceMock.Verify(c => c.UpdateHistory(id, operation, expectedOutput, CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task SubtractionService_Subtract_Should_Pass()
        {
            var historyServiceMock = new Mock<IHistoryClient>();
            var calculatorRepositoryMock = new Mock<ICalculatorRepository>();
            var calculatorSetMock = new Mock<DbSet<CalculatorRecord>>();
            var auditRepositoryMock = new Mock<IAuditRepository<SubtractionAuditRecord>>();
            var auditSetMock = new Mock<DbSet<SubtractionAuditRecord>>();

            calculatorRepositoryMock.Setup(c => c.Calculators).Returns(calculatorSetMock.Object);
            auditRepositoryMock.Setup(c => c.Audits).Returns(auditSetMock.Object);

            var value = 5M;
            var operation = Operations.Subtraction;
            var expectedResult = -5M;

            historyServiceMock
                .Setup(c => c.UpdateHistory(It.IsAny<Guid>(), operation, -value, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() { CalculatorId = Guid.NewGuid(), NewValue = expectedResult, OldValue = null, Operation = Operations.Subtraction });

            var service = new SubtractionService(calculatorRepositoryMock.Object, auditRepositoryMock.Object, historyServiceMock.Object);
            var result = await service.Subtract(null, value, CancellationToken.None);

            Assert.AreEqual(expectedResult, result.Result);
            Assert.AreNotEqual(result.GlobalId, default(Guid));

            calculatorRepositoryMock.Verify(r => r.Add(It.IsAny<CalculatorRecord>(), CancellationToken.None), Times.Once);
            auditRepositoryMock.Verify(r => r.AddAudit(It.IsAny<SubtractionAuditRecord>(), CancellationToken.None), Times.Once);
            historyServiceMock.Verify(c => c.UpdateHistory(It.IsAny<Guid>(), operation, expectedResult, CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task SubtractionService_Subtract_Existing_Id_Should_Pass()
        {
            var historyServiceMock = new Mock<IHistoryClient>();
            var calculatorRepositoryMock = new Mock<ICalculatorRepository>();
            var calculatorSetMock = new Mock<DbSet<CalculatorRecord>>();
            var auditRepositoryMock = new Mock<IAuditRepository<SubtractionAuditRecord>>();
            var auditSetMock = new Mock<DbSet<SubtractionAuditRecord>>();

            calculatorRepositoryMock.Setup(c => c.Calculators).Returns(calculatorSetMock.Object);
            auditRepositoryMock.Setup(c => c.Audits).Returns(auditSetMock.Object);

            var id = Guid.NewGuid();
            var value = 10M;
            var operand = 5M;
            var operation = Operations.Subtraction;
            var expectedOutput = value - operand;

            calculatorRepositoryMock
                .Setup(c => c.Find(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(new CalculatorRecord { Id = id, Value = value });
            historyServiceMock
                .Setup(c => c.UpdateHistory(id, operation, expectedOutput, CancellationToken.None))
                .ReturnsAsync(new HistoryDto() { CalculatorId = id, NewValue = expectedOutput, OldValue = value, Operation = Operations.Subtraction });

            var service = new SubtractionService(calculatorRepositoryMock.Object, auditRepositoryMock.Object, historyServiceMock.Object);
            var result = await service.Subtract(id, operand, CancellationToken.None);

            Assert.AreEqual(expectedOutput, result.Result);
            Assert.AreEqual(id, result.GlobalId);

            calculatorRepositoryMock.Verify(r => r.Update(It.IsAny<CalculatorRecord>(), CancellationToken.None), Times.Once);
            auditRepositoryMock.Verify(r => r.AddAudit(It.IsAny<SubtractionAuditRecord>(), CancellationToken.None), Times.Once);
            historyServiceMock.Verify(c => c.UpdateHistory(It.IsAny<Guid>(), operation, expectedOutput, CancellationToken.None), Times.Once);
        }
    }
}
