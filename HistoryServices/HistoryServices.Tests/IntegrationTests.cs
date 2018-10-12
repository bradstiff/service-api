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
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Collections.Generic;

namespace HistoryServices.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private HttpClient HttpClient { get; } = new HttpClient() { BaseAddress = new Uri("https://localhost:44318/api/history/") };

        [TestMethod]
        public async Task GetLast_Two_Adds_Should_Pass()
        {
            var key = $"Calculator[{Guid.NewGuid()}]";
            var value1 = new HistoryViewModel
            {
                Operation = "add",
                NewValue = "5",
            };
            var value2 = new HistoryViewModel
            {
                Operation = "subtract",
                NewValue = "3",
            };

            var result1 = await this.Add(key, value1);
            Assert.IsTrue(result1.IsSuccessStatusCode);

            var result2 = await this.Add(key, value2);
            Assert.IsTrue(result2.IsSuccessStatusCode);

            var add2 = await result2.Content.ReadAsAsync<HistoryViewModel>();
            Assert.AreEqual("3", add2.NewValue);
            Assert.AreEqual("5", add2.OldValue);

            var result3 = await this.GetLast(key);
            Assert.IsTrue(result3.IsSuccessStatusCode);

            var last = await result3.Content.ReadAsAsync<HistoryViewModel>();
            Assert.AreEqual(add2.Id, last.Id);
            Assert.AreEqual(add2.NewValue, last.NewValue);
        }

        [TestMethod]
        public async Task GetAll_Two_Adds_Should_Pass()
        {
            var key = $"Calculator[{Guid.NewGuid()}]";
            var value1 = new HistoryViewModel
            {
                Operation = "add",
                NewValue = "5",
            };
            var value2 = new HistoryViewModel
            {
                Operation = "subtract",
                NewValue = "3",
            };

            var result1 = await this.Add(key, value1);
            Assert.IsTrue(result1.IsSuccessStatusCode);

            var result2 = await this.Add(key, value2);
            Assert.IsTrue(result2.IsSuccessStatusCode);

            var result3 = await this.GetAll(key);
            Assert.IsTrue(result3.IsSuccessStatusCode);

            var all = await result3.Content.ReadAsAsync<IEnumerable<HistoryViewModel>>();
            var first = all.First();
            var last = all.Last();

            Assert.AreEqual(first.NewValue, last.OldValue);
            Assert.AreNotEqual(first.Id, last.Id);
        }

        private Task<HttpResponseMessage> Add(string key, HistoryViewModel value)
        {
            return this.HttpClient.PostAsync<HistoryViewModel>(key, value, new JsonMediaTypeFormatter());
        }

        private Task<HttpResponseMessage> GetAll(string key)
        {
            return this.HttpClient.GetAsync(key);
        }

        private Task<HttpResponseMessage> GetLast(string key)
        {
            return this.HttpClient.GetAsync($"{key}/last");
        }
    }
}
