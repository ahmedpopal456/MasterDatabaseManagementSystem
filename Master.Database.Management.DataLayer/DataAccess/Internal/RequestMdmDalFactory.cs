using System;
using AutoMapper;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal.Fields;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal.Sections;

namespace Master.Database.Management.DataLayer.DataAccess.Internal
{
  public class RequestMdmDalFactory : IRequestMdmDalFactory
  {
    private readonly MdmContext _mdmContext;
    private readonly IMapper _mapper;

    public RequestMdmDalFactory(MdmContext mdmContext, IMapper mapper)
    {
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(RequestMdmDalFactory)} expects a value for {nameof(mapper)}... null argument was provided");
      _mdmContext = mdmContext ?? throw new ArgumentNullException($"{nameof(RequestMdmDalFactory)} expects a value for {nameof(mdmContext)}... null argument was provided");
    }

    public IMdmFieldDal RequestMdmFieldDal()
    {
      return new MdmFieldDal(_mdmContext, _mapper);
    }

    public IMdmFixCategoryDal RequestMdmFixCategoryDal()
    {
      return new MdmFixCategoryDal(_mdmContext, _mapper);
    }

    public IMdmFixTemplateDal RequestMdmFixTemplateDal()
    {
      return new MdmFixTemplateDal(_mdmContext, _mapper);
    }

    public IMdmFixTypeDal RequestMdmFixTypeDal()
    {
      return new MdmFixTypeDal(_mdmContext, _mapper);
    }

    public IMdmSectionDal RequestMdmSectionDal()
    {
      return new MdmSectionDal(_mdmContext, _mapper);
    }
  }
}
