using Functions.Function;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Helpers;
using Xunit;




namespace Test.Test
{
    public class ScheduledFunctionTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();


        [Fact]
        public void ScheduledFunctionTest_Should_Log_Messaga()
        {

            // Arrenge 
            MockCloudTable mockTodos = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            MockCloudTable mockTodos2 = new MockCloudTable(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            ListLogger logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);
            // Act
            ScheduledFunction.Run(null, mockTodos, mockTodos2, logger);
            string message = logger.Logs[0];

            // Assert
            Assert.Contains("Tabla llena completed", message);
        }
    }
}
