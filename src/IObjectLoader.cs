namespace BuilderScenario
{
    public interface IObjectLoader
    {
        T Load<T>(string configRelativePath);
    }
}