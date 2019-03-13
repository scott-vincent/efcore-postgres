using Amazon.DynamoDBv2.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace aws_dynamodb.Database
{
    [DynamoDBTable("Employees")]
    public class Employee
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? Birthdate { get; set; }
        public decimal? Salary { get; set; }
    }
}
