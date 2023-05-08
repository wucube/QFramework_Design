
namespace QFramework
{
    public interface ICanSendEvent:ICanGetArchitecture { }

    public static class CanSendEventExtension
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        public static void SendEvent<T>(this ICanSendEvent self, T e) where T : new()
        {
            self.GetArchitecture().SendEvent<T>(e);
        }
    }

}
