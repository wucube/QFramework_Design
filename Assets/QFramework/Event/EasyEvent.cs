using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{ 
  /// <summary>
  /// 轻量事件接口
  /// </summary>
  public interface IEasyEvent { }          
  
  /// <summary>
  /// 轻量事件类
  /// </summary>
  public class EasyEvent : IEasyEvent     
  {         
    /// <summary>
    /// 委托成员
    /// </summary>
    /// <returns></returns>
    private Action mOnEvent = () => { };          
    
    public IUnRegister Register(Action onEvent)         
    {             
      mOnEvent += onEvent;             
      
      return new CustomUnRegister(() => { UnRegister(onEvent); });         
    }          
    
    public void UnRegister(Action onEvent)         
    {             
      mOnEvent -= onEvent;         
    }          
    
    public void Trigger()         
    {             
      mOnEvent?.Invoke();         
    }     
  }      

  public class EasyEvent<T> : IEasyEvent     
  {         
    private Action<T> mOnEvent = e => { };          
    
    public IUnRegister Register(Action<T> onEvent)         
    {             
      mOnEvent += onEvent;             
      
      return new CustomUnRegister(() => { UnRegister(onEvent); });         
    }          
    
    public void UnRegister(Action<T> onEvent)         
    {             
      mOnEvent -= onEvent;         
    }          
    
    public void Trigger(T t)         
    {             
      mOnEvent?.Invoke(t);         
    }     
  }      

  public class EasyEvent<T, K> : IEasyEvent     
  {         
    private Action<T, K> mOnEvent = (t, k) => { };          
    
    public IUnRegister Register(Action<T, K> onEvent)         
    {             
      mOnEvent += onEvent;             
      
      return new CustomUnRegister(() => { UnRegister(onEvent); });         
    }          
    
    public void UnRegister(Action<T, K> onEvent)         
    {             
      mOnEvent -= onEvent;         
    }          
    
    public void Trigger(T t, K k)         
    {             
      mOnEvent?.Invoke(t, k);         
    }     
  }      

  public class EasyEvent<T, K, S> : IEasyEvent     
  {         
    private Action<T, K, S> mOnEvent = (t, k, s) => { };          
    
    public IUnRegister Register(Action<T, K, S> onEvent)         
    {             
      mOnEvent += onEvent;             
      
      return new CustomUnRegister(() => { UnRegister(onEvent); });         
    }          
    
    public void UnRegister(Action<T, K, S> onEvent)         
    {             
      mOnEvent -= onEvent;         
    }          
    
    public void Trigger(T t, K k, S s)         
    {             
      mOnEvent?.Invoke(t, k, s);         
    }     
  }      

  public class EasyEvents     
  {         
    private static EasyEvents mGlobalEvents = new EasyEvents();          
    
    public static T Get<T>() where T : IEasyEvent         
    {             
      return mGlobalEvents.GetEvent<T>();         
    }                   
    
    public static void Register<T>() where T : IEasyEvent, new()         
    {             
      mGlobalEvents.AddEvent<T>();         
    }          
    
    private Dictionary<Type, IEasyEvent> mTypeEvents = new Dictionary<Type, IEasyEvent>();                  
    
    public void AddEvent<T>() where T : IEasyEvent, new()         
    {             
      mTypeEvents.Add(typeof(T), new T());         
    }          
    
    public T GetEvent<T>() where T : IEasyEvent         
    {             
      IEasyEvent e;              
      
      if (mTypeEvents.TryGetValue(typeof(T), out e))             
      {                 
        return (T)e;             
      }              
      
      return default;         
    }          
    
    public T GetOrAddEvent<T>() where T : IEasyEvent, new()         
    {             
      var eType = typeof(T);             
      
      if (mTypeEvents.TryGetValue(eType, out var e))             
      {                 
        return (T)e;             
      }              
      
      var t = new T();             
      
      mTypeEvents.Add(eType, t);             
      
      return t;         
    }
  }
     
}      
