﻿using Common.Enums;
using Functions.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Function
{
  public  class ScheduledFunctionTest
    {
        private static CloudTable _cloudTable;

        [FunctionName("ScheduledFunction")]
        public static async Task Run(
          [TimerTrigger("0 * * * * *")] TimerInfo myTimer,
          [Table("employee", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
          [Table("consolidated", Connection = "AzureWebJobsStorage")] CloudTable consolidateTable,
          ILogger log)
        {
            _cloudTable = cloudTable;
            log.LogInformation($"Deling completed function executed at: {DateTime.Now}");
            string filter = TableQuery.GenerateFilterConditionForBool("Consolidated", QueryComparisons.Equal, false);
            TableQuery<EmployeeEntity> query = new TableQuery<EmployeeEntity>().Where(filter);
            TableQuerySegment<EmployeeEntity> employees = await cloudTable.ExecuteQuerySegmentedAsync(query, null);

            List<EmployeeEntity> entryEmployees = employees.ToList().Where(e => e.Type == (int)TypeEnum.Entry).ToList();
            List<EmployeeEntity> outputEmployees = employees.ToList().Where(e => e.Type == (int)TypeEnum.Output).ToList();

            foreach (EmployeeEntity itemEntry in entryEmployees)
            {
                EmployeeEntity itemOutput = outputEmployees.FirstOrDefault(e => 
                e.IdEmployee == itemEntry.IdEmployee && !e.Consolidated);
                if (itemOutput != null)
                {
                    TimeSpan difference = itemOutput.Date - itemEntry.Date;
                    ConsolidatedEntity consolidatedEntity = new ConsolidatedEntity
                    {
                        Date = DateTime.UtcNow,
                        ETag = "*",
                        IdEmployee = itemEntry.IdEmployee,
                        PartitionKey = "CONSOLIDATED",
                        RowKey = Guid.NewGuid().ToString(),
                        WorkTime = difference,
                    };

                    TableOperation addOperation = TableOperation.Insert(consolidatedEntity);
                    await consolidateTable.ExecuteAsync(addOperation);
                    log.LogInformation($"Insert Consolidate: {itemOutput.IdEmployee} item at: {DateTime.Now}");

                    itemOutput.Consolidated = true;
                    itemEntry.Consolidated = true;
                    await UpdateEmployee(itemOutput);
                    await UpdateEmployee(itemEntry);
                    log.LogInformation($"Update employee consolidate true: {itemOutput.IdEmployee} item at: {DateTime.Now}");
                }
            }
        }

        private static async Task UpdateEmployee(EmployeeEntity itemOutput)
        {
            TableOperation addOperation = TableOperation.Replace(itemOutput);
            await _cloudTable.ExecuteAsync(addOperation);
        }
    }
}
