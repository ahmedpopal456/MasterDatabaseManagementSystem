using System;
using Fixit.Core.DataContracts.FixTemplates;

namespace Master.Database.Management.DataLayer.DataAccess.Models.Filters
{
  public class FixTemplateFilterDto : FilterBaseDto
  {
    public Guid? UserId { get; set; }

    public string[] Tags { get; set; }

    public FixTemplateStatus? Status { get; set; }

    public string TypeName { get; set; }

    public string CategoryName { get; set; }

    public string UnitName { get; set; }
  }
}
