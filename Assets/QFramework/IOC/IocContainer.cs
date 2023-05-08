using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    [Obsolete("请使用 SimpleIoc",false)]
    public class IocContainer
    {
        //实例容器
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        //注册 
        public void Register<T>(T instance)
        {
            var key = typeof(T);
            if (mInstances.ContainsKey(key))
                mInstances[key] = instance;
            else
                mInstances.Add(key,instance);
        }

        //获取
        public T Get<T>() where T : class
        {
            var key = typeof(T);
            //查询
            if(mInstances.TryGetValue(key,out var retInstance))
                return retInstance as T;
        
            return null;
        }
    }
}

