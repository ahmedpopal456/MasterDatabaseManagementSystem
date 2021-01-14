using System;
using System.Collections.Generic;
using System.Text;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Master.Database.Management.DataLayer.Models;

namespace Master.Database.Management.ServerlessApi.Seeders.Internal.Operations
{
  internal class FakeFixTemplateCreateRequestDtoSeeder : IFakeSeederAdapter<FixTemplateCreateRequestDto>
  {
    public IList<FixTemplateCreateRequestDto> SeedFakeDtos()
    {
      return new List<FixTemplateCreateRequestDto>
      {
        new FixTemplateCreateRequestDto()
        {
          Status = FixTemplateStatus.Private,
          Name = "Fix Template Name 1",
          CategoryId = new Guid("96371910-43e3-4621-98c2-2396cd663e0c"),
          TypeId = new Guid("445e50d1-b2e7-4c25-a628-c610aed7a357"),
          Description = "A new FixTemplateCreateRequestDto",
          SystemCostEstimate = 123.45,
          CreatedByUserId = Guid.NewGuid(),
          UpdatedByUserId = Guid.NewGuid(),
          Tags = new List<string>()
          {
            "Tag 1",
            "Tag 2"
          },
          Sections = new List<FixTemplateSectionCreateRequestDto>
          {
            new FixTemplateSectionCreateRequestDto()
            {
              Name = "Section 1",
              Fields = new List<FixTemplateFieldCreateRequestDto>
              {
                new FixTemplateFieldCreateRequestDto()
                {
                  Name = "Field 1.1",
                  Values = new List<string>
                  {
                    "Value 1.1.1",
                    "Value 1.1.2"
                  }
                }
              }
            }
          }
        }
      };
    }
  }
}
