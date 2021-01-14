using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal.Fields;
using Master.Database.Management.DataLayer.DataAccess.Internal.FixTemplates.Internal.Sections;

namespace Master.Database.Management.DataLayer.DataAccess
{
  /// <summary>
  /// <para>An MDM Dal factory.</para>
  /// <para>Allows the creation of any MDM Dals.</para>
  /// </summary>
  public interface IRequestMdmDalFactory
	{
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

    /// <summary>
    /// Creates an instance of the <see cref="MdmFixCategoryDal"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM FixCategory Dal: <see cref="IMdmFixCategoryDal"/></returns>
		public IMdmFixCategoryDal RequestMdmFixCategoryDal();

    /// <summary>
    /// Creates an instance of the <see cref="MdmFixTypeDal"/> with its dependencies. 
    /// </summary>
    /// <returns>The interface of the MDM FixType Dal: <see cref="IMdmFixTypeDal"/></returns>
		public IMdmFixTypeDal RequestMdmFixTypeDal();
	}
}
