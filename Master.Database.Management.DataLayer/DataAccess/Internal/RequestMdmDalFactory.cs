using System;
using AutoMapper;
using Master.Database.Management.DataLayer.DataAccess.Classifications;
using Master.Database.Management.DataLayer.DataAccess.FixTemplates;
using Master.Database.Management.DataLayer.DataAccess.Internal.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Fields;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Sections;

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

    public IMdmSkillDal RequestMdmSkillDal()
    {
      return new MdmSkillDal(_mdmContext, _mapper);
    }

    #region Fix Templates
    public IMdmFieldDal RequestMdmFieldDal()
    {
      return new MdmFieldDal(_mdmContext, _mapper);
    }

    public IMdmSectionDal RequestMdmSectionDal()
    {
      return new MdmSectionDal(_mdmContext, _mapper);
    }

    public IMdmFixTemplateDal RequestMdmFixTemplateDal()
    {
      return new MdmFixTemplateDal(_mdmContext, _mapper);
    }
    #endregion

    #region Classifications
    public IMdmWorkTypeDal RequestMdmWorkTypeDal()
    {
      return new MdmWorkTypeDal(_mdmContext, _mapper);
    }

    public IMdmWorkCategoryDal RequestMdmWorkCategoryDal()
    {
      return new MdmWorkCategoryDal(_mdmContext, _mapper);
    }

    public IMdmFixUnitDal RequestMdmFixUnitDal()
    {
      return new MdmFixUnitDal(_mdmContext, _mapper);
    }
    #endregion
  }
}
