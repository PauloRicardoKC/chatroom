namespace Chat.UI.Configurations.MessageBus
{
    public interface IDestinationQueueMapper
    {
        IDestinationQueueMapper Map<T>() where T : class;
    }
}