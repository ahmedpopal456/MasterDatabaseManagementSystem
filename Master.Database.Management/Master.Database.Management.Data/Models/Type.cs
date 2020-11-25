using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Master.Database.Management.Data.Models
{
    public class Type
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
