namespace QFramework
{
    public interface ISystem:ICanGetArchitecture,ICanSetArchitecture,ICanGetModel,ICanGetUtility,ICanSendEvent,ICanRegisterEvent,ICanGetSystem
    {
        void Init();
    }

    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture mArchitecture;
        private ISystem _systemImplementation;

        IArchitecture ICanGetArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }
    
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture as IArchitecture;
        }

        void  ISystem.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();
    }
}
