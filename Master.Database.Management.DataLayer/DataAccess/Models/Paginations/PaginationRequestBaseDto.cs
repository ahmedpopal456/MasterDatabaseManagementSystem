using Master.Database.Management.DataLayer.DataAccess.Models.Filters;

namespace Master.Database.Management.DataLayer.DataAccess.Models.Paginations
{
  public class PaginationRequestBaseDto : FilterBaseDto, IPaginationRequestDto
  {
    public int PageNumber { get; set; }

    public int? PageSize { get; set; }
  }
}
