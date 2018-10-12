using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

using HistoryServices.Data.Core;
using Microsoft.EntityFrameworkCore;
using HistoryServices.Data;
using HistoryServices.Services;
using HistoryServices.ViewModels;
using System.Threading;
using System;
using System.Linq;

namespace HistoryServices.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public async Task Add_Should_Pass()
        {
            var repositoryMock = new Mock<IHistoryRepository>();
            var mockDbSet = new Mock<DbSet<HistoryRecord>>();
            repositoryMock
                .Setup(c => c.Histories)
                .Returns(mockDbSet.Object);

            var value = new HistoryViewModel
            {
                Operation = "add",
                NewValue = "5"
            };
            var key = "Calculator[1]";

            var service = new HistoryService(repositoryMock.Object);
            await service.Add(key, value, CancellationToken.None);
            var result = await service.Add(key, value, CancellationToken.None);

            Assert.AreEqual("add", result.Operation);
            Assert.AreEqual(null, result.OldValue);
            Assert.AreEqual("5", result.NewValue);
        }

        [TestMethod]
        public async Task GetAll_Empty_History_Should_Pass()
        {
            var repositoryMock = new Mock<IHistoryRepository>();
            var mockDbSet = new Mock<DbSet<HistoryRecord>>();
            repositoryMock
                .Setup(c => c.Histories)
                .Returns(mockDbSet.Object);

            var key = "Calculator[1]";

            var service = new HistoryService(repositoryMock.Object);
            var result = await service.GetAll(key, CancellationToken.None);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task GetLast_Empty_History_Should_Pass()
        {
            var repositoryMock = new Mock<IHistoryRepository>();
            var mockDbSet = new Mock<DbSet<HistoryRecord>>();
            repositoryMock
                .Setup(c => c.Histories)
                .Returns(mockDbSet.Object);

            var key = "Calculator[1]";

            var service = new HistoryService(repositoryMock.Object);
            var result = await service.GetLast(key, CancellationToken.None);

            Assert.IsNull(result);
        }
    }
}
