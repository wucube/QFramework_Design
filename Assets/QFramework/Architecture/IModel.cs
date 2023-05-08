using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace QFramework
{
    public interface IModel:ICanGetArchitecture,ICanSetArchitecture,ICanGetUtility,ICanSendEvent
    {
        void Init();
    }

    public abstract class AbstractModel : IModel
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

        //接口阉割技术
        void  IModel.Init()
        {   
            OnInit();
        }
        protected abstract void OnInit();
    }
}

