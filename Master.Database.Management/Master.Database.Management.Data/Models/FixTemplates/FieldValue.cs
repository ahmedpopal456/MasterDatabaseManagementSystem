using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Database.Management.Data.Models.FixTemplates
{
    public class FieldValue
    {
        [ForeignKey("Field")]
        public Guid FieldId { get; set; }

        [ForeignKey("Value")]
        public Guid ValueId { get; set; }

        public virtual Field Field { get; set; }

        public virtual Value Value { get; set; }
    }
}
