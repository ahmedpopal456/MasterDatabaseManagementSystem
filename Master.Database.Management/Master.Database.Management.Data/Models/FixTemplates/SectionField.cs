using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Database.Management.Data.Models.FixTemplates
{
    public class SectionField
    {
        //TODO specify composite key (FixTemplateId, SectionId, FieldId) in Context

        [ForeignKey("FixTemplate")]
        public Guid FixTemplateId { get; set; }

        [ForeignKey("Section")]
        public Guid SectionId { get; set; }

        [ForeignKey("Field")]
        public Guid FieldId { get; set; }

        public virtual FixTemplate FixTemplate { get; set; }

        public virtual Section Section { get; set; }

        public virtual Field Field { get; set; }
    }
}
