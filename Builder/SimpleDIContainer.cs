using System.Reflection;

namespace BuilderScenario
{
    public interface IServiceCollection
    {
        T Resolve<T>() where T : class;
        object[] ResolveArguments(MethodInfo method);
    }
}