using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Master.Database.Management.Data.Models.FixTemplates
{
    public class Value
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
