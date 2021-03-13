namespace Master.Database.Management.DataLayer.DataAccess.Models.Filters
{
  public class FilterBaseDto
  {
    public string Name { get; set; }

    public long? MinTimestampUtc { get; set; }

    public long? MaxTimestampUtc { get; set; }
  }
}
