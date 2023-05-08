using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public abstract class ShootingEditor2DController : MonoBehaviour,IController
    {
        //使用接口阉割，相对严谨的写法
        IArchitecture  ICanGetArchitecture.GetArchitecture()
        {
            return ShootingEditor2D.Interface;
        }
    }

}
