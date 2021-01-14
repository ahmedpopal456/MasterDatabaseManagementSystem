using System;
using System.Collections.Generic;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Fixes.Categories;
using Fixit.Core.DataContracts.Fixes.Types;
using Fixit.Core.DataContracts.FixTemplates;

namespace Master.Database.Management.ServerlessApi.Seeders.Internal.Fixes
{
  internal class FakeFixTemplateDtoSeeder : IFakeSeederAdapter<FixTemplateDto>
  {
    public IList<FixTemplateDto> SeedFakeDtos()
    {
      return new List<FixTemplateDto>
      {
        new FixTemplateDto()
        {
          Id = new Guid("ba60c506-05ae-4d7f-a68c-48a60599bdf6"),
          Status = FixTemplateStatus.Public,
          Name = "Fix Template Name 1",
          Category = new FixCategoryDto() 
          {
            Id = new Guid("96371910-43e3-4621-98c2-2396cd663e0c"),
            Name = "Bathroom"
          },
          Type = new FixTypeDto()
          {
            Id = new Guid("445e50d1-b2e7-4c25-a628-c610aed7a357"),
            Name = "New"
          },
          Description = "A fix template that already exists.",
          SystemCostEstimate = 123.45,
          CreatedByUserId = Guid.NewGuid(),
          UpdatedByUserId = Guid.NewGuid(),
          Tags = new List<string>()
          {
            "Tag 1",
            "Tag 2"
          },
          Sections = new List<FixTemplateSectionDto>
          {
            new FixTemplateSectionDto()
            {
              SectionId = new Guid("0b7d5429-cad0-487f-40f8-08d8af5a64f5"),
              Name = "Section 1",
              Fields = new List<FixTemplateFieldDto>
              {
                new FixTemplateFieldDto()
                {
                  Id = new Guid("9be203c5-d1c1-4ee5-e1e8-08d8af5a6528"),
                  Name = "Field 1.1",
                  Values = new List<string>
                  {
                    "Value 1.1.1",
                    "Value 1.1.2"
                  }
                },
                new FixTemplateFieldDto()
                {
                  Id = new Guid("ad5ceffd-89cb-4a8d-e1e9-08d8af5a6528"),
                  Name = "Field 1.2",
                  Values = new List<string>
                  {
                    "Value 1.2.1",
                    "Value 1.2.2"
                  }
                },
              }
            }
          }
        }
      };
    }
  }
}
