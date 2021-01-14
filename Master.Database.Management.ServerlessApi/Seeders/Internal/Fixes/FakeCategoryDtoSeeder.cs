using System;
using System.Collections.Generic;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Fixes.Categories;

namespace Master.Database.Management.ServerlessApi.Seeders.Internal.Fixes
{
  internal class FakeCategoryDtoSeeder : IFakeSeederAdapter<FixCategoryDto>
  {
    public IList<FixCategoryDto> SeedFakeDtos()
    {
      return new List<FixCategoryDto>
      {
        new FixCategoryDto()
        {
          Id = new Guid("96371910-43e3-4621-98c2-2396cd663e0c"),
          Name = "Bathroom"
        },
        new FixCategoryDto()
        {
          Id = new Guid("51a1e09e-bf7e-48d1-ac51-538d6f1bb957"),
          Name = "Bedroom"
        },
        new FixCategoryDto()
        {
          Id = new Guid("211609c1-3584-4454-aa3d-60cb40d618be"),
          Name = "DiningRoom",
        },
        new FixCategoryDto()
        {
          Id = new Guid("b9860dba-01c8-4d2f-8256-703845c246ee"),
          Name = "Kitchen"
        }
      };
    }
  }
}
