using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CalculatorServices.Controllers;
using CalculatorServices.Services;
using CalculatorServices.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorServices.Tests.Integration
{
    [TestClass]
    public class IntegrationTests
    {
        private static readonly HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44354/api/calculator/") };

        #region Addition
        [TestMethod]
        public async Task CalculatorController_Add_No_Value_Should_Fail()
        {
            var result = await Post(Operations.Addition);

            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task CalculatorController_Add_No_Id_Value_5_Should_Pass()
        {
            var value = 5;

            var result = await Post(Operations.Addition, value: value);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var content = await result.Content.ReadAsAsync<CalculatorResultViewModel>();

            Assert.AreEqual(value, content.Result);
        }

        [TestMethod]
        public async Task CalculatorController_Add_Same_Id_Twice_Should_Pass()
        {
            var value = 5;

            var result = await Post(Operations.Addition, value: value);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "StatusCode should be OK or 200");

            var content1 = await result.Content.ReadAsAsync<CalculatorResultViewModel>();

            Assert.AreEqual(value, content1.Result, $"Result should be 0 + {value}");

            result = await Post(Operations.Addition, id: content1.GlobalId, value: value);

            var content2 = await result.Content.ReadAsAsync<CalculatorResultViewModel>();

            Assert.AreEqual(content1.GlobalId, content2.GlobalId, "Global Ids should be the same");
            Assert.AreEqual(value + value, content2.Result, $"Result should be {value} + {value}");
        }
        #endregion

        #region Subtraction
        [TestMethod]
        public async Task CalculatorController_Subtract_No_Value_Should_Fail()
        {
            var result = await Post(Operations.Subtraction);

            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task CalculatorController_Subtract_No_Id_Value_5_Should_Pass()
        {
            var value = 5;

            var result = await Post(Operations.Subtraction, value: value);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var content = await result.Content.ReadAsAsync<CalculatorResultViewModel>();

            Assert.AreEqual(-value, content.Result);
        }

        [TestMethod]
        public async Task CalculatorController_Subtract_Same_Id_Twice_Should_Pass()
        {
            var value = 5;

            var result = await Post(Operations.Subtraction, value: value);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "StatusCode should be OK or 200");

            var content1 = await result.Content.ReadAsAsync<CalculatorResultViewModel>();

            Assert.AreEqual(-value, content1.Result, $"Result should be {-value}");

            result = await Post(Operations.Subtraction, id: content1.GlobalId, value: value);

            var content2 = await result.Content.ReadAsAsync<CalculatorResultViewModel>();

            Assert.AreEqual(content1.GlobalId, content2.GlobalId, "Global Ids should be the same");
            Assert.AreEqual(0 - value - value, content2.Result, $"Result should be 0 - {value} - {value}");
        }
        #endregion

        #region History
        [TestMethod]
        public async Task CalculatorController_GetHistory_NoId_Should_Fail()
        {
            var result = await Get(null);

            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task CalculatorController_GetHistory_Two_Operations_Should_Pass()
        {
            var value = 5;
            
            var operation1 = await Post(Operations.Addition, value: value);
            var content1 = await operation1.Content.ReadAsAsync<CalculatorResultViewModel>();

            var operation2 = await Post(Operations.Subtraction, content1.GlobalId, value);
            var content2 = await operation2.Content.ReadAsAsync<CalculatorResultViewModel>();

            var result = await Get(content2.GlobalId);

            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            var history = await result.Content.ReadAsAsync<CalculatorHistoryViewModel>();

            Assert.AreEqual(2, history.Operations.Count());
            
            var firstStep = history.Operations.First();
            var lastStep = history.Operations.Last();

            Assert.AreNotEqual(firstStep, lastStep);
            
            Assert.AreEqual(Operations.Addition.ToString(), firstStep.Operation);
            Assert.AreEqual(Operations.Subtraction.ToString(), lastStep.Operation);

            Assert.IsNull(firstStep.OldValue);
            Assert.AreEqual(value, firstStep.NewValue);

            Assert.AreEqual(firstStep.NewValue, lastStep.OldValue);
            Assert.AreEqual(0, lastStep.NewValue);
        }

        #endregion

        private static Task<HttpResponseMessage> Get(Guid? id)
        {
            if (id.HasValue)
            {
                return _httpClient.GetAsync($"history/{id}");
            }

            return _httpClient.GetAsync($"history");
        }

        private static Task<HttpResponseMessage> Post(Operations operation, Guid? id = null, decimal? value = null)
        {
            var parameters = new Dictionary<string, string>();

            if (id.HasValue)
            {
                parameters.Add("id", id.Value.ToString());
            }

            if (value.HasValue)
            {
                parameters.Add("value", value.Value.ToString());
            }

            var queryString = new StringBuilder();

            switch (operation)
            {
                case Operations.Addition:
                    queryString.Append("add");
                    break;
                case Operations.Subtraction:
                    queryString.Append("subtract");
                    break;
                default:
                    throw new ArgumentException(nameof(operation));
            }

            queryString.Append("?");

            foreach (var parameter in parameters)
            {
                queryString.AppendFormat($"&{parameter.Key}={parameter.Value}");
            }

            return _httpClient.PostAsync(queryString.ToString(), null);
        }
    }
}
