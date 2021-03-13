using Master.Database.Management.DataLayer.DataAccess.Classifications;
using Master.Database.Management.DataLayer.DataAccess.FixTemplates;
using Master.Database.Management.DataLayer.DataAccess.Internal;
using Master.Database.Management.DataLayer.DataAccess.Internal.Classifications;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Fields;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Sections;

namespace Master.Database.Management.DataLayer.DataAccess
{
  /// <summary>
  /// <para>An MDM Dal factory.</para>
  /// <para>Allows the creation of any MDM Dals.</para>
  /// </summary>
  public interface IRequestMdmDalFactory
	{
    /// <summary>
    /// Creates an instance of the <see cref="MdmSkillDal"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Skill Dal: <see cref="IMdmSkillDal"/></returns>
		public IMdmSkillDal RequestMdmSkillDal();

    #region Fix Templates
    /// <summary>
    /// Creates an instance of the <see cref="MdmFieldDal"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Field Dal: <see cref="IMdmFieldDal"/></returns>
		public IMdmFieldDal RequestMdmFieldDal();

    /// <summary>
    /// Creates an instance of the <see cref="MdmSectionDal"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Section Dal: <see cref="IMdmSectionDal"/></returns>
		public IMdmSectionDal RequestMdmSectionDal();

    /// <summary>
    /// Creates an instance of the <see cref="MdmFixTemplateDal"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM FixTemplate Dal: <see cref="IMdmFixTemplateDal"/></returns>
		public IMdmFixTemplateDal RequestMdmFixTemplateDal();
    #endregion

    #region Classifications
    /// <summary>
    /// Creates an instance of the <see cref="MdmWorkTypeDal"/> with its dependencies. 
    /// </summary>
    /// <returns>The interface of the MDM WorkType Dal: <see cref="IMdmWorkTypeDal"/></returns>
		public IMdmWorkTypeDal RequestMdmWorkTypeDal();

    /// <summary>
    /// Creates an instance of the <see cref="MdmWorkCategoryDal"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM WorkCategory Dal: <see cref="IMdmWorkCategoryDal"/></returns>
    public IMdmWorkCategoryDal RequestMdmWorkCategoryDal();

    /// <summary>
    /// Creates an instance of the <see cref="MdmFixUnitDal"/> with its dependencies. 
    /// </summary>
    /// <returns>The interface of the MDM FixUnit Dal: <see cref="IMdmFixUnitDal"/></returns>
		public IMdmFixUnitDal RequestMdmFixUnitDal();
    #endregion
  }
}
