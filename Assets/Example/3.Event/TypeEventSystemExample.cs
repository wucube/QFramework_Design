using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example
{
    public class TypeEventSystemExample : MonoBehaviour
    {
        public struct EventA { }
        public struct EventB
        {
            public int ParamB;
        }
        public interface IEventGroup { }
        public struct EventC:IEventGroup { }
        public struct EventD:IEventGroup { }
    
        private TypeEventSystem mTypeEventSystem = new TypeEventSystem();
    
        void Start()
        {
            mTypeEventSystem.Register<EventA>(OnEventA);
            
            //当前脚本挂载的gameobject销毁后自动注销
            mTypeEventSystem.Register<EventB>(b =>
            {
                Debug.Log("OnEventB:"+b.ParamB);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            
            mTypeEventSystem.Register<IEventGroup>(e =>
            {
                Debug.Log(e.GetType());
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            
        }
    
        private void OnEventA(EventA obj)
        {
            Debug.Log("OnEventA");
        }
    
        void Update()
        {
            if(Input.GetMouseButtonDown(0)) mTypeEventSystem.Send<EventA>();
            if(Input.GetMouseButtonDown(1)) mTypeEventSystem.Send(new EventB(){ParamB = 123});
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mTypeEventSystem.Send<IEventGroup>(new EventC());
                mTypeEventSystem.Send<IEventGroup>(new EventD());
            }
        }
    
        void OnDestroy()
        {
            mTypeEventSystem.UnRegister<EventA>(OnEventA);
            mTypeEventSystem = null;
        }
    }
}

