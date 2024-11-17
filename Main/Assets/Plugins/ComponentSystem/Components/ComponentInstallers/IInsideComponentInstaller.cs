namespace ComponentSystem
{
    public interface IInsideComponentInstaller
    {
        IComponent[] InstallInside(IComponent sender);
    }
}
