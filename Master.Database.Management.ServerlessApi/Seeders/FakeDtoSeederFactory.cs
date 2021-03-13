using System.Collections.Generic;
using Fixit.Core.DataContracts.Seeders;

namespace Master.Database.Management.ServerlessApi.Seeders
{
  public class FakeDtoSeederFactory : IFakeSeederFactory
  {
    public IList<T> CreateSeederFactory<T>(IFakeSeederAdapter<T> fakeSeederAdapter) where T : class
    {
      return fakeSeederAdapter.SeedFakeDtos();
    }
  }
}
