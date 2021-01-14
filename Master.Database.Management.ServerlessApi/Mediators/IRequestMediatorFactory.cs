
namespace Master.Database.Management.ServerlessApi.Mediators
{
  /// <summary>
  /// <para>An MDM Mediator factory.</para>
  /// <para>Allows the creation of any MDM Mediators.</para>
  /// </summary>
  public interface IRequestMediatorFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="Master.Database.Management.ServerlessApi.Mediators.Internal.Fixes.Templates.FixTemplateMediator"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Field Dal: <see cref="IFixTemplateMediator"/></returns>
    public IFixTemplateMediator RequestFixTemplateMediator();

    /// <summary>
    /// Creates an instance of the <see cref="Master.Database.Management.ServerlessApi.Mediators.Internal.Fixes.Types.FixTypeMediator"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Field Dal: <see cref="IFixTypeMediator"/></returns>
    public IFixTypeMediator RequestFixTypeMediator();

    /// <summary>
    /// Creates an instance of the <see cref="Master.Database.Management.ServerlessApi.Mediators.Internal.Fixes.Categories.FixCategoryMediator"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Field Dal: <see cref="IFixCategoryMediator"/></returns>
    public IFixCategoryMediator RequestFixCategoryMediator();
  }
}
