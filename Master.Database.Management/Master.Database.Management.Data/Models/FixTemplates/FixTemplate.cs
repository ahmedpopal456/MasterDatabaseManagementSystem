using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Master.Database.Management.Data.Models.FixTemplates
{
    public class FixTemplate
    {
        [Key]
        public Guid Id { get; set; }

        public Status Status { get; set; }

        public string Name { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        [ForeignKey("Type")]
        public Guid TypeId { get; set; }

        public string Description { get; set; }

        //TODO link to User model
        public Guid CreatedById { get; set; }

        //TODO link to User model
        public Guid UpdatedById { get; set; }

        //? timestamp type
        public long CreatedOn { get; set; }

        //? timestamp type
        public long UpdatedOn { get; set; }

        //? timestamp type
        public long LastAccessedOn { get; set; }

        public double SystemCostEstimate { get; set; }

        public int Frequency { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Category Category { get; set; }

        public virtual Type Type { get; set; }

    }
}
