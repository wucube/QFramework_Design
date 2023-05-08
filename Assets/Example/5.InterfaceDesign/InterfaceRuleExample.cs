using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDoEverything
{
    public void DoSomething1()
    {
        Debug.Log("DoSomething1");
    }
    public void DoSomething2()
    {
        Debug.Log("DoSomething2");
    }
    public void DoSomething3()
    {
        Debug.Log("DoSomething3");
    }
}
public interface IHasEveryThing
{
    CanDoEverything CanDoEverything { get; }
}

public interface ICanDoSomething1 : IHasEveryThing { }
public interface ICanDoSomething2 : IHasEveryThing { }
public interface ICanDoSomething3 : IHasEveryThing { }


//静态扩展
public static class ICanDoSomething1Extension
{
    //DoSomething1的方法扩展
    public static void DoSomething1(this ICanDoSomething1 self)
    {
        self.CanDoEverything.DoSomething1();
    }
}
public static class ICanDoSomething2Extension
{
    //DoSomething2的方法扩展
    public static void DoSomething2(this ICanDoSomething2 self)
    {
        self.CanDoEverything.DoSomething2();
    }
}
public static class ICanDoSomething3Extension
{
    //DoSomething3的方法扩展
    public static void DoSomething3(this ICanDoSomething3 self)
    {
        self.CanDoEverything.DoSomething3();
    }
}

public class InterfaceRuleExample : MonoBehaviour
{
    //一个接口是一个规则
    public class OnlyCanDo1:ICanDoSomething1
    {
        CanDoEverything IHasEveryThing.CanDoEverything { get; } = new CanDoEverything();
    }
    
    public class OnlyCanDo23:ICanDoSomething2,ICanDoSomething3
    {
        CanDoEverything IHasEveryThing.CanDoEverything { get;} =  new CanDoEverything();
    }

    private void Start()
    {
        var onlyCanDo1 = new OnlyCanDo1();
        onlyCanDo1.DoSomething1();

        var onlyCanDo23 = new OnlyCanDo23();
        onlyCanDo23.DoSomething2();
        onlyCanDo23.DoSomething3();
    }
}
