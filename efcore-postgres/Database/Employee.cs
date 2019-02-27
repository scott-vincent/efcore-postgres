using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace efcore_postgres
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime? Birthdate { get; set; }
        public decimal? Salary { get; set; }
    }
}
