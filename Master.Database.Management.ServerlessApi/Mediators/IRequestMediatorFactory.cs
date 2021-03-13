
namespace Master.Database.Management.ServerlessApi.Mediators
{
  /// <summary>
  /// <para>An MDM Mediator factory.</para>
  /// <para>Allows the creation of any MDM Mediators.</para>
  /// </summary>
  public interface IRequestMediatorFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="Internal.FixTemplates.FixTemplateMediator"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Field Dal: <see cref="IFixTemplateMediator"/></returns>
    public IFixTemplateMediator RequestFixTemplateMediator();

    /// <summary>
    /// Creates an instance of the <see cref="Internal.Classifications.FixUnitMediator"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Field Dal: <see cref="IFixUnitMediator"/></returns>
    public IFixUnitMediator RequestFixUnitMediator();

    /// <summary>
    /// Creates an instance of the <see cref="Internal.SkillMediator"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Field Dal: <see cref="ISkillMediator"/></returns>
    public ISkillMediator RequestSkillMediator();

    /// <summary>
    /// Creates an instance of the <see cref="Internal.Classifications.WorkCategoryMediator"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Field Dal: <see cref="IWorkCategoryMediator"/></returns>
    public IWorkCategoryMediator RequestWorkCategoryMediator();

    /// <summary>
    /// Creates an instance of the <see cref="Internal.Classifications.WorkTypeMediator"/> with its dependencies.
    /// </summary>
    /// <returns>The interface of the MDM Field Dal: <see cref="IWorkTypeMediator"/></returns>
    public IWorkTypeMediator RequestWorkTypeMediator();
  }
}
