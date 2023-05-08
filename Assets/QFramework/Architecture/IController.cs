using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IController:ICanGetArchitecture,ICanSendCommand,ICanGetSystem,ICanGetModel,ICanRegisterEvent,ICanSendQuery
    {
    }

}
