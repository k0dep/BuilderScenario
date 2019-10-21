namespace BuilderScenario
{
    public interface IJobExecuteService
    {
        JobExecuteResult Execute(IBuildJob job);
    }
}