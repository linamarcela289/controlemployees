using Common.Models;
using Functions.Entities;
using Functions.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Helpers;
using Xunit;

namespace Test.Test
{
    public class ApiTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        //[Fact]
        //public async void CreateEmployee_Should_Return_200()
        //{
        //    // Arrenge 
        //    MockCloudTable mockTodos = new MockCloudTable (new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
        //    Employee employeeRequest = TestFactory.GetEmployeeRequest();  
        //    DefaultHttpRequest request = TestFactory.CreateHttpRequest(employeeRequest);

        //    // Act
        //    IActionResult response = await Api.CreateEntryOrOutputEmployee(request, mockTodos, logger);

        //    // Assert
        //    OkObjectResult result = (OkObjectResult)response;
        //    Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        //}

        //[Fact]
        //public async void UpdateEmployee_Should_Return_200()
        //{
        //    // Arrenge 
        //    MockCloudTable mockTodos = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
        //    Employee employeeRequest = TestFactory.GetEmployeeRequest();  
        //    Guid employeeId = Guid.NewGuid();
        //    DefaultHttpRequest request = TestFactory.CreateHttpRequest(employeeId, employeeRequest);

        //    // Act
        //    IActionResult response = await Api.UpdateEmployee(request, mockTodos, employeeId.ToString(), logger);

        //    // Assert
        //    OkObjectResult result = (OkObjectResult)response;
        //    Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        //}

        //[Fact]
        //public async void DeleteEmployee_Should_Return_200()
        //{
        //    // Arrenge 
        //    MockCloudTable mockEmployee = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
        //    Employee employeeRequest = TestFactory.GetEmployeeRequest();
        //    Guid employeeId = Guid.NewGuid();
        //    DefaultHttpRequest request = TestFactory.DeleteHttpRequest(employeeId);

        //    // Act
        //    IActionResult response = await Api.DeleteEmployee(request, TestFactory.GetEmployeeEntity(),
        //        mockEmployee, TestFactory.GetEmployeeEntity().IdEmployee.ToString(), logger);

        //    // Assert
        //    OkObjectResult result = (OkObjectResult)response;
        //    Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        //}

        //[Fact]
        //public async void GetAllEmployeeId_Should_Return_200()
        //{
        //    // Arrenge 
        //    MockCloudTable mockEmployee = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
        //    Employee employeeRequest = TestFactory.GetEmployeeRequest();
        //    Guid employeeId = Guid.NewGuid();
        //    DefaultHttpRequest request = TestFactory.GetEmployeeHttpRequestId(employeeId);

        //    // Act
        //    IActionResult response = Api.GetEmployeeById(request, TestFactory.GetEmployeeEntity(), 
        //        TestFactory.GetEmployeeEntity().IdEmployee.ToString(), logger);

        //    // Assert
        //    OkObjectResult result = (OkObjectResult)response;
        //    Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        //}


       [Fact]
        public async void GetAllEmployee_Should_Return_200()
       {
          // Arrenge 
            MockCloudTable mockEmployee = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
         // Employee employeeRequest = TestFactory.GetEmployeeRequest();
           Guid employeeId = Guid.NewGuid();
           DefaultHttpRequest request = TestFactory.GetEmployeeHttpRequestId(employeeId);

            // Act
            IActionResult response = await Api.GetAllEmployee(request, mockEmployee, logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
         public async void GetAllConsolidatedforDate_Should_Return_200()
         {
             // Arrenge 
             MockCloudTable mockConsolidated = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
             DefaultHttpRequest candidatedRequest = TestFactory.GetConsolidateddRequestDate(DateTime.Today);

             // Act
             IActionResult response = await Api.GetAllConsolidatedforDate(candidatedRequest, mockConsolidated, TestFactory.GetConsolidatedEntity(),
                  DateTime.Today, logger);

             // Assert
             OkObjectResult result = (OkObjectResult)response;
             Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
         }

    }
}
