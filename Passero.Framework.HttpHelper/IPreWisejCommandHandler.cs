namespace Passero.Framework.HttpHelper 
{
    public interface IPreWisejCommandHandler
    {
        PreWisejCommandDefinition Definition { get; }

        bool TryHandle(
            PreWisejRequestData request,
            out PreWisejCommandResult result);
    }
}