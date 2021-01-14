using System.Net.Http;
using Fixit.Core.DataContracts;

namespace Master.Database.Management.ServerlessApi.Seeders
{
  public class FakeHttpRequestMessageAdapter : FakeHttpRequestMessageAdapterBase
  {
    protected override HttpRequestMessage CreateFakeHttpRequestMessage(ByteArrayContent content)
    {
      return new HttpRequestMessage()
      {
        Content = content
      };
    }
  }
}
