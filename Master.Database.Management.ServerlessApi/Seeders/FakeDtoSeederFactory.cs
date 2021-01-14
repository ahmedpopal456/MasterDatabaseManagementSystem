using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Fixes.Categories;
using Fixit.Core.DataContracts.Fixes.Types;
using Fixit.Core.DataContracts.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Fields;
using Fixit.Core.DataContracts.FixTemplates.Operations.Requests.FixTemplates;
using Fixit.Core.DataContracts.FixTemplates.Sections;
using Master.Database.Management.ServerlessApi.Seeders.Internal.Fixes;
using Master.Database.Management.ServerlessApi.Seeders.Internal.Operations;

namespace Master.Database.Management.ServerlessApi.Seeders
{
  public class FakeDtoSeederFactory : IFakeSeederFactory
  {
    public IFakeSeederAdapter<T> CreateFakeSeeder<T>() where T : class
    {
      string type = typeof(T).Name;

      return type switch
      {
        nameof(FixCategoryDto) => (IFakeSeederAdapter<T>)new FakeCategoryDtoSeeder(),
        nameof(FixTypeDto) => (IFakeSeederAdapter<T>)new FakeTypeDtoSeeder(),
        nameof(SectionDto) => (IFakeSeederAdapter<T>)new FakeSectionDtoSeeder(),
        nameof(FieldDto) => (IFakeSeederAdapter<T>)new FakeFieldDtoSeeder(),
        nameof(FixTemplateDto) => (IFakeSeederAdapter<T>)new FakeFixTemplateDtoSeeder(),
        nameof(FixTemplateCreateRequestDto) => (IFakeSeederAdapter<T>)new FakeFixTemplateCreateRequestDtoSeeder(),
        nameof(FixTemplateUpdateRequestDto) => (IFakeSeederAdapter<T>)new FakeFixTemplateUpdateRequestDtoSeeder(),
        _ => null,
      };
    }
  }
}
