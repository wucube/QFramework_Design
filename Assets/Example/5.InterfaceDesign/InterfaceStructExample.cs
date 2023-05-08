using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceStructExample : MonoBehaviour
{
    //接口
    public interface ICustomScript
    {
        void Start();
        void Update();
        void Destroy();
    }
    
    //抽象类
    public abstract class CustomScript:ICustomScript
    {
        //显式实现接口方法，不希望子类访问这些方法
        void ICustomScript.Start() { OnStart(); }
        void ICustomScript.Update() { OnUpdate(); }
        void ICustomScript.Destroy() { OnDestroy(); }

        //由子类实现的方法
        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnDestroy();
    }

    //用户扩展的类
    public class MyScript : CustomScript
    {
        protected override void OnStart()
        {
            Debug.Log("On Start");
        }
        protected override void OnUpdate()
        {
            Debug.Log("On Update");
        }
        protected override void OnDestroy()
        {
            Debug.Log("On Destroy");
        }
    }

    void Start()
    {
        ICustomScript myScript = new MyScript();
        myScript.Start();
        myScript.Update();
        myScript.Destroy();
    }
}
