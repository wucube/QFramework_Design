using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// 类型事件系统
    /// </summary>
    public interface ITypeEventSystem
    {   //发送事件
        void Send<T>() where T : new();
        void Send<T>(T e);
        //注册事件,注册时返回注销对象，方便使用
        IUnRegister Register<T>(Action<T> onEvent);
        //注销事件
        void UnRegister<T>(Action<T> onEvent);
    }

    /// <summary>
    /// 注销事件接口
    /// </summary>
    public interface IUnRegister
    {
        /// <summary>
        /// 注销事件
        /// </summary>
        void UnRegister();
    }
    /// <summary>
    /// 注销列表接口
    /// </summary>
    public interface IUnRegisterList
    {
        /// <summary>
        /// 注销事件列表
        /// </summary>
        /// <value></value>
        List<IUnRegister> UnRegisterList { get; }
    }
    /// <summary>
    /// 注销事件列表扩展
    /// </summary>
    public static class IUnRegisterListExtension
    {
        /// <summary>
        /// 添加到注销事件列表
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unRegisterList"></param>
        public static void AddToUnregisterList(this IUnRegister self, IUnRegisterList unRegisterList)
        {
            unRegisterList.UnRegisterList.Add(self);
        }

        /// <summary>
        /// 注销所有事件
        /// </summary>
        /// <param name="self"></param>
        public static void UnRegisterAll(this IUnRegisterList self)
        {
            foreach (var unRegister in self.UnRegisterList)
            {
                unRegister.UnRegister();
            }

            self.UnRegisterList.Clear();
        }
    }
    
    /// <summary>
    /// 自定义可注销的类
    /// </summary>
    public struct CustomUnRegister : IUnRegister
    {
        /// <summary>
        /// 委托对象
        /// </summary>
        private Action mOnUnRegister { get; set; }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="onDispose"></param>
        public CustomUnRegister(Action onUnRegsiter)
        {
            mOnUnRegister = onUnRegsiter;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void UnRegister()
        {
            mOnUnRegister.Invoke();
            mOnUnRegister = null;
        }
    }

    //GameObject销毁时，会自动注销注册的消息或事件
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private readonly HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Add(unRegister);
        }

        public void RemoveUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Remove(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in mUnRegisters)
            {
                unRegister.UnRegister();
            }

            mUnRegisters.Clear();
        }
    }

    public static class UnRegisterExtension
    {
        public static IUnRegister UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            trigger.AddUnRegister(unRegister);
            
            return unRegister;
        }
        
        public static IUnRegister UnRegisterWhenGameObjectDestroyed<T>(this IUnRegister self, T component)
            where T : Component
        {
            return self.UnRegisterWhenGameObjectDestroyed(component.gameObject);
        }
    }

    public class TypeEventSystem //基于EasyEvent重构的TypeEventSystem
    {         
        private readonly EasyEvents mEvents = new EasyEvents();           
  
        //便于使用TypeEventSystem对象
        public static readonly TypeEventSystem Global = new TypeEventSystem();          
  
        public void Send<T>() where T : new()         
        {             
            mEvents.GetEvent<EasyEvent<T>>()?.Trigger(new T());         
        }          
  
        public void Send<T>(T e)         
        {             
            mEvents.GetEvent<EasyEvent<T>>()?.Trigger(e);         
        }          
  
        public IUnRegister Register<T>(Action<T> onEvent)         
        {             
            var e = mEvents.GetOrAddEvent<EasyEvent<T>>();             
    
            return e.Register(onEvent);         
        }          
  
        public void UnRegister<T>(Action<T> onEvent)         
        {             
            var e = mEvents.GetEvent<EasyEvent<T>>();             
    
            if (e != null)             
            {                 
                e.UnRegister(onEvent);             
            }         
        }     
    } 
}

