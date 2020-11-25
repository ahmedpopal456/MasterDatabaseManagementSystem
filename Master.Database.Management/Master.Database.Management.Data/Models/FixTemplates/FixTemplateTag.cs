using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Master.Database.Management.Data.Models.FixTemplates
{
    public class FixTemplateTag
    {
        //TODO specify composite key (FixTemplateId, TagName) in Context

        [ForeignKey("FixTemplate")]
        public Guid FixTemplateId { get; set; }

        public string TagName { get; set; }

        public virtual FixTemplate FixTemplate { get; set; }
    }
}
