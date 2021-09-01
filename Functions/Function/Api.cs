using Common.Models;
using Common.Responses;
using Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Functions.Function
{
    public class Api
    {
        [FunctionName(nameof(CreateEntryOrOutputEmployee))]
        public static async Task<IActionResult> CreateEntryOrOutputEmployee(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employee")] HttpRequest req,
           [Table("employee", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
           ILogger log)

        {
            log.LogInformation("Recived new employee");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Employee employee = JsonConvert.DeserializeObject<Employee>(requestBody);

            if (employee?.IdEmployee == 0)
            {

                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "The application must have an employee"
                });
            }


            EmployeeEntity employeeEntity = new EmployeeEntity
            {
                
                Consolidated = false,
                Date = employee.Date.Value,
                IdEmployee = employee.IdEmployee,
                ETag = "*",
                PartitionKey = "EMPLOYEE",
                RowKey = Guid.NewGuid().ToString(),
                Type = (int)employee.Type
            };

            TableOperation addOperation = TableOperation.Insert(employeeEntity);
            await cloudTable.ExecuteAsync(addOperation);

            string message = "New register stored in table";
            log.LogInformation(message);


            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = employeeEntity
            });
        }

        [FunctionName(nameof(GetAllEmployee))]
        public static async Task<IActionResult> GetAllEmployee(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employee")] HttpRequest req,
          [Table("employee", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
          ILogger log)

        {
            log.LogInformation("Get all employee received.");

            TableQuery<EmployeeEntity> query = new TableQuery<EmployeeEntity>();
            TableQuerySegment<EmployeeEntity> employees = await cloudTable.ExecuteQuerySegmentedAsync(query, null);

            string message = "Retrieved all employee";
            log.LogInformation(message);


            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = employees
            });
        }

        [FunctionName(nameof(DeleteEmployee))]
        public static async Task<IActionResult> DeleteEmployee(
         [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "employee/{id}")] HttpRequest req,
         [Table("employee", "EMPLOYEE", "{id}", Connection = "AzureWebJobsStorage")] EmployeeEntity employeeEntity,
         [Table("employee", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
         string id,
         ILogger log)
        {
            log.LogInformation($"Delete  employee: {id}, received.");

            if (employeeEntity == null)
            {
                return new NotFoundObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Employee not found."
                });
            }

            await cloudTable.ExecuteAsync(TableOperation.Delete(employeeEntity));
            string message = $"Employee: {employeeEntity.RowKey}, deleted.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = employeeEntity
            });
        }

        [FunctionName(nameof(UpdateEmployee))]
        public static async Task<IActionResult> UpdateEmployee(
           [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "employee/{id}")] HttpRequest req,
           [Table("employee", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
           string id,
           ILogger log)

        {
            log.LogInformation($"Update for employee: {id}, received.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Employee employee = JsonConvert.DeserializeObject<Employee>(requestBody);
            TableOperation findOperation = TableOperation.Retrieve<EmployeeEntity>("EMPLOYEE", id);
            TableResult findResult = await cloudTable.ExecuteAsync(findOperation);

            if (findResult.Result == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "employee not found."

                });
            }

            //Update employee
            EmployeeEntity employeeEntity = (EmployeeEntity)findResult.Result;
            employeeEntity.Type = (int)employee.Type;
            if (employee.Date.HasValue)
            {
                employeeEntity.Date = employee.Date.Value;
            }

            if (employee.Consolidated.HasValue)
            {
                employeeEntity.Consolidated = employee.Consolidated.Value;
            }

            TableOperation addOperation = TableOperation.Replace(employeeEntity);
            await cloudTable.ExecuteAsync(addOperation);

            string message = $"Employee: {id}, Update in table";
            log.LogInformation(message);


            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = employeeEntity
            });
        }

        [FunctionName(nameof(GetEmployeeById))]
        public static IActionResult GetEmployeeById(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employee/{id}")] HttpRequest req,
         [Table("employee", "EMPLOYEE", "{id}")] EmployeeEntity employeeEntity,
         string id,
         ILogger log)


        {
            log.LogInformation($"Get employee by id: {id}, received.");

            if (employeeEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Employeer not found."

                });
            }

            string message = $"Employeer: {employeeEntity.RowKey}, retrieved";
            log.LogInformation(message);


            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = employeeEntity
            });
        }


        [FunctionName(nameof(GetAllConsolidatedforDate))]
        public static async Task<IActionResult> GetAllConsolidatedforDate(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "consolidated/{date}")] HttpRequest req,
          [Table("consolidated", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
          [Table("consolidated", "CONSOLIDATED", "{date}")] ConsolidatedEntity consolidatedEntity,
          DateTime date,
          ILogger log)

        {
            log.LogInformation("Get all consolidated received.");
            string filter = TableQuery.GenerateFilterConditionForDate("Date", QueryComparisons.Equal, date);
            TableQuery<ConsolidatedEntity> query = new TableQuery<ConsolidatedEntity>().Where(filter);
            TableQuerySegment<ConsolidatedEntity> consolidateds = await cloudTable.ExecuteQuerySegmentedAsync(query, null);

            string message = $"Retrieved all consolidatedEntity";
            log.LogInformation(message);


            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = consolidateds
            });
        }

    }
}



