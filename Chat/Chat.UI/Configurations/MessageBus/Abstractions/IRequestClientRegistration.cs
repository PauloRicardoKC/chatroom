namespace Chat.UI.Configurations.MessageBus
{
    public interface IRequestClientRegistration
    {
        IRequestClientRegistration Add<T>() where T : class;
    }
}