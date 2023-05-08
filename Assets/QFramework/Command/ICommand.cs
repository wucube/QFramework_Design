using System;

namespace QFramework
{
    public interface ICommand:ICanGetArchitecture,ICanSetArchitecture,ICanGetSystem,ICanGetModel,ICanGetUtility,ICanSendEvent,ICanSendCommand,ICanSendQuery
    {
        void Execute();
    }

    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture mArchitecture;
   
        IArchitecture ICanGetArchitecture.GetArchitecture()
        {   
            return mArchitecture;
        }
   
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }

        void ICommand.Execute()
        {   
            OnExecute();
        }
        protected abstract void OnExecute();
    }
}

