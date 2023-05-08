using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// 简易IOC容器接口，替代第一季实现的IOCContainer
    /// </summary>
    public interface ISimpleIoc
    {
        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Register<T>();

        /// <summary>
        /// 注册为单例
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterInstance<T>(object instance);

        /// <summary>
        /// 注册为单例
        /// </summary>
        /// <param name="instance"></param>
        void RegisterInstance(object instance);

        /// <summary>
        /// 注册依赖
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <returns></returns>
        void Register<TBase, TConcrete>() where TConcrete : TBase;

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// 注入对象
        /// </summary>
        /// <param name="obj"></param>
        void Inject(object obj);

        /// <summary>
        /// 清空容器
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// 自定义简易IOC注入的特性
    /// </summary>
    public class SimpleIOCInjectAttribute : Attribute { }

    /// <summary>
    /// 简易IOC容器
    /// </summary>
    public class SimpleIOC : ISimpleIoc
    {
        /// <summary>
        /// 存储不同注册类型的容器
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <returns></returns>
        private HashSet<Type> mRegisteredType = new HashSet<Type>();

        /// <summary>
        /// 存储Instance的容器
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <typeparam name="object"></typeparam>
        /// <returns></returns>
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        /// <summary>
        /// 存储接口实现依赖的容器
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <typeparam name="Type"></typeparam>
        /// <returns></returns>
        private Dictionary<Type, Type> mDependencies = new Dictionary<Type, Type>();

        public void Register<T>()
        {
            if(!mRegisteredType.Contains(typeof(T)))
                //将类型添加到HashSet中
                mRegisteredType.Add(typeof(T));
            else
                Debug.Log("mRegisteredType已包含该类型");

        }

        public void RegisterInstance<T>(object instance)
        {
            if (mInstances.ContainsKey(typeof(T)))
                mInstances[typeof(T)] = instance;
            else 
                mInstances.Add(typeof(T),instance);
        }

        public void RegisterInstance(object instance)
        {
            //获取类型并存储
            var type = instance.GetType();
            
            if (mInstances.ContainsKey(type))
                mInstances[type] = instance;
            else
                mInstances.Add(type,instance);
        }

        public void Register<TBase, TConcrete>() where TConcrete : TBase
        {
            var baseType = typeof(TBase);
            var concreteType = typeof(TConcrete);
            
            if (mDependencies.ContainsKey(baseType))
                mDependencies[baseType] = concreteType;
            else
                mDependencies.Add(baseType,concreteType);
        }

       
        public T Resolve<T>()
        {
            var type = typeof(T);

            return (T)Resolve(type);
        }

        //将原来 T Resolve<T>() 中的根据类型创建实例的部分提出来，供 Inject() 调用
        public object Resolve(Type type)
        {
            if (mInstances.ContainsKey(type))
                return mInstances[type];

            if (mDependencies.ContainsKey(type))
                return Activator.CreateInstance(mDependencies[type]);
             
            //若包含该类型就通过反射创建实例
            if (mRegisteredType.Contains(type))
                return Activator.CreateInstance(type);

            return default;
        }

        public void Inject(object obj)
        {
            foreach (var propertyInfo in obj.GetType().GetProperties().Where(p=>p.GetCustomAttributes(typeof(SimpleIOCInjectAttribute)).Any()))
            {
                var instance = Resolve(propertyInfo.PropertyType);
                
                if(instance!=null)//通过反射赋值
                    propertyInfo.SetValue(obj,instance);
                else
                    Debug.LogFormat("不能获取类型为:{0} 的对象",propertyInfo.PropertyType);
            }
        }

        public void Clear()
        {
            mRegisteredType.Clear();
            mInstances.Clear();
            mDependencies.Clear();
        }
    }

}
