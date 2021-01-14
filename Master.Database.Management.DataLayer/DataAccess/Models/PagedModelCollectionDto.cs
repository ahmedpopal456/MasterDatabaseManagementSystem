using System.Collections.Generic;

namespace Master.Database.Management.DataLayer.DataAccess.Models
{
  public class PagedModelCollectionDto<T> 
  {
    private IList<T> _results; 

    public IList<T> Results
    {
      get => _results ??= new List<T>();
      set => _results = value; 
    }

    public int PageNumber { get; set; }

    public int TotalModelCount { get; set; } 
  }
}
