using System.Reflection;

namespace BuilderScenario
{
    public interface IServiceCollection
    {
        T Resolve<T>() where T : class;
        void Inject(object target);
        object[] ResolveArguments(MethodInfo method);
    }
}