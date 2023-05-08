using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

namespace QFramework
{
    /// <summary>
    /// 架构接口
    /// </summary>
    public interface IArchitecture
    {
        /// <summary>
        /// 注册System
        /// </summary>
        /// <param name="system"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterSystem<T>(T system) where T : ISystem;

        /// <summary>
        /// 注册Model
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterModel<T>(T model) where T : IModel;

        /// <summary>
        /// 注册Utility
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterUtility<T>(T utility) where T:IUtility;

        /// <summary>
        /// 获取System
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T GetSystem<T>() where T : class, ISystem;

        /// <summary>
        /// 获取Utility
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T GetUtility<T>() where T : class,IUtility;

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T GetModel<T>() where T : class, IModel;

        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;

        /// <summary>
        /// 发送查询
        /// </summary>
        /// <param name="query"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        TResult SendQuery<TResult>(IQuery<TResult> query);
        void SendEvent<T>() where T : new();
        void SendEvent<T>(T e);
        
        IUnRegister RegisterEvent<T>(Action<T> onEvent);
        void UnRegisterEvent<T>(Action<T> onEvent);

    }
    /// <summary>
    /// 抽象架构类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>,new()
    {
        /// <summary>
        /// 静态架构类
        /// </summary>
        private static T mArchitecture;

        /// <summary>
        /// 是否完成初始化
        /// </summary>
        private bool mInited = false;
        
        /// <summary>
        /// 缓存要初始化的Model，便于统一初始化
        /// </summary>
        /// <typeparam name="IModel"></typeparam>
        /// <returns></returns>
        private List<IModel> mModels = new List<IModel>();
        /// <summary>
        /// 缓存用于初始化的Systems
        /// </summary>
        /// <typeparam name="ISystem"></typeparam>
        /// <returns></returns>
        private List<ISystem> mSystems = new List<ISystem>();

        /// <summary>
        /// 将Architecture类型传出去的委托
        /// </summary>
        /// <value></value>
        public static Action<T> OnRegisterPatch = architecture => { };
        
        /// <summary>
        /// 返回mArchitecture引用的实例
        /// </summary>
        /// <value></value>
        public static IArchitecture Interface
        {
            get
            {
                if(mArchitecture == null) 
                    MakeSureArchitecture();

                return mArchitecture;
            }
        }

        /// <summary>
        /// 确保mArchitecture有实例
        /// </summary>
        static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();

                //注册模块
                mArchitecture.Init();

                //将Architecture传出去
                OnRegisterPatch?.Invoke(mArchitecture);

                //逐个初始化缓存的Model
                foreach (var architectureModel in mArchitecture.mModels)
                {
                    //初始化所有注册的模块
                    architectureModel.Init();
                }
                //清空缓存的Model
                mArchitecture.mModels.Clear();
              
                //初始化System
                foreach (var architectureSystems in mArchitecture.mSystems)
                {
                    architectureSystems.Init();
                }
                //清空System
                mArchitecture.mSystems.Clear();
                
                //初始化完成
                mArchitecture.mInited = true;
            }
        }
        
        /// <summary>
        /// IOC容器
        /// </summary>
        /// <returns></returns>
        private SimpleIOC mContainer = new SimpleIOC();

        /// <summary>
        /// 在子类中注册模块
        /// </summary>
        protected abstract void Init();
        
        /// <summary>
        /// 注册Model
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="TModel"></typeparam>
        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            //为Model的Architecture赋值，把自己设置进去
            model.SetArchitecture(this);
            
            mContainer.RegisterInstance<TModel>(model);
            
            //初始化未完成时注册Model 缓存注册完的Model
            if(!mInited) mModels.Add(model);
            //初始化完成后注册Model,直接初始化Model
            else model.Init();
        }
        
        /// <summary>
        /// 注册System
        /// </summary>
        /// <param name="system"></param>
        /// <typeparam name="TSystem"></typeparam>
        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchitecture(this);
            mContainer.RegisterInstance<TSystem>(system);
            if(!mInited) mSystems.Add(system);
            else system.Init();
        }

        /// <summary>
        /// 注册Utility,在CounterApp类内部使用
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="TUtility"></typeparam>
        public void RegisterUtility<TUtility>(TUtility utility) where TUtility:IUtility
        {
            mContainer.RegisterInstance<TUtility>(utility);
        }

        /// <summary>
        /// 获取System
        /// </summary>
        /// <typeparam name="TSystem"></typeparam>
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return mContainer.Resolve<TSystem>();
        }
        
        /// <summary>
        /// 获取Utility
        /// </summary>
        /// <typeparam name="TUtility"></typeparam>
        public TUtility GetUtility<TUtility>() where TUtility : class,IUtility
        {
            //获取Utility(工具)的方法
            return mContainer.Resolve<TUtility>();
        }

        /// <summary>
        /// 获取Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return mContainer.Resolve<TModel>();
        } 
        
        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            var command = new TCommand();
            //先为Architecture赋值
            command.SetArchitecture(this);
            command.Execute();
            //执行完去掉引用。
            //在命令执行中，使用TimeSystem延时发送新命令，会报空出错，故先不清空引用。
            //在2D射击项目中，打完枪内子弹自动装填时，会空引用异常 TODO：在清空引用后，嵌套的延时命令不出错
            //command.SetArchitecture(null); 
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
            //command.SetArchitecture(null);
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }

        private TypeEventSystem mTypeEventSystem = new TypeEventSystem();
        
        public void SendEvent<TEvent>() where TEvent : new()
        {
            mTypeEventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(TEvent e)
        {
            mTypeEventSystem.Send<TEvent>(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return mTypeEventSystem.Register<TEvent>(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            mTypeEventSystem.UnRegister<TEvent>(onEvent);
        }
    }
}

