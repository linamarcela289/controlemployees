using Common.Models;
using Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Test.Helpers
{
    public class TestFactory
    {
        public static EmployeeEntity GetEmployeeEntity()
        {
            return new EmployeeEntity
            {
                Consolidated = false,
                Date = DateTime.UtcNow,
                IdEmployee = 200,
                ETag = "*",
                PartitionKey = "EMPLOYEE",
                RowKey = Guid.NewGuid().ToString(),
                Type = 0
            };
        }

        public static ConsolidatedEntity GetConsolidatedEntity()
        {
            return new ConsolidatedEntity
            {

                Date = DateTime.Today,
                IdEmployee = 200,
                ETag = "*",
                PartitionKey = "CONSOLIDATED",
                RowKey = Guid.NewGuid().ToString(),
                WorkTime = 5
                
            };
        }


        public static DefaultHttpRequest CreateHttpRequest(Guid employeeId, Employee employeeRequest)
        {
            string request = JsonConvert.SerializeObject(employeeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{employeeId}"
            };
        }
        public static DefaultHttpRequest DeleteHttpRequest(Guid employeeId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{employeeId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Employee employeeRequest)
        {
            string request = JsonConvert.SerializeObject(employeeRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }

        public static DefaultHttpRequest GetConsolidateddRequestDate(DateTime date)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{date}"
            };
        }

        public static DefaultHttpRequest GetEmployeeHttpRequestId(Guid employeeId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{employeeId}"
            };
        }

       
        public static Employee GetEmployeeRequest()
        {
            return new Employee
            {
                IdEmployee = 90,
                Consolidated = false,
                Type = 0,
                Date = DateTime.UtcNow

            };
        }
    }


}

