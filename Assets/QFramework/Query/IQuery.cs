using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QFramework
{
    public interface IQuery<TResult>:ICanGetArchitecture,ICanSetArchitecture,ICanGetModel,ICanGetSystem,ICanSendQuery
    {
        TResult Do();
    }

    public abstract class AbstractQuery<T> : IQuery<T>
    {
        public T Do()
        {
            return OnDo();
        }
        protected abstract T OnDo();

        private IArchitecture mArchitecture;

        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
}
