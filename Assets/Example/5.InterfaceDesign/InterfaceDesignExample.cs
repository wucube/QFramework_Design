  using System;
  using System.Collections;
using System.Collections.Generic;
using UnityEngine;


  public interface ICanSyaHello
  {
    void SayHello();
    void SayOther();
  }
  public class InterfaceDesignExample : MonoBehaviour,ICanSyaHello
  {
    //接口隐式实现
    public void SayHello()
    {
       Debug.Log("Hello");
    }

    //接口显式实现
    void ICanSyaHello.SayOther()
    {
       Debug.Log("Other");
    }
    private void Start()
    {
       this.SayHello();
       
       //显式实现的接口方法必须通过接口调用
       (this as ICanSyaHello).SayHello();
    }
  }
