using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public interface ITimeSystem:ISystem
    {
        float CurrentSeconds { get; }
        void AddDelayTask(float seconds, Action onDelayFinish);
    }

    public class DelayTask
    {
        public float Seconds { get; set; }
        public Action OnFinish { get; set; }
        public float StartSeconds { get; set; }
        public float FinishSeconds { get; set; }
        public DelayTaskState State { get; set; }
    }

    public enum DelayTaskState
    {
        NotStart,
        Started,
        Finish
    }
    
    public class TimeSystem : AbstractSystem, ITimeSystem
    {
        
        public class TimeSystemUpdateBehaviour:MonoBehaviour
        {
            public event Action OnUpdate;
            private void Update()
            {
                OnUpdate?.Invoke();
            }
        }
        protected override void OnInit()
        {
            //新建GameObject,挂载TimeSystemUpdateBehaviour脚本
            var updateBehaviourGameObj = new GameObject(nameof(TimeSystemUpdateBehaviour));
            
            //让计时gameobjct持久存在，才能在下次开始游戏后继续计时
            GameObject.DontDestroyOnLoad(updateBehaviourGameObj);
            
            var updateBehaviour = updateBehaviourGameObj.AddComponent<TimeSystemUpdateBehaviour>();
            
            //决定版架构的系统层只考虑了初始化，未实现销毁退订
            //自己实现销毁，需要设置Mono脚本的成员变量
            updateBehaviour.OnUpdate += OnUpdate;
            
            CurrentSeconds = 0;
        }

        void OnUpdate()
        {
            //计时
            CurrentSeconds += Time.deltaTime;

            if (mDelayTasks.Count > 0)
            {
                var currentNode = mDelayTasks.First;
                
                while (currentNode!=null)//第二个节点可能为空，要先判断
                {
                    var nextNode = currentNode.Next;
                    var delayTask = currentNode.Value;
                    if (delayTask.State == DelayTaskState.NotStart)
                    {
                        delayTask.State = DelayTaskState.Started;
                        delayTask.StartSeconds = CurrentSeconds;
                        delayTask.FinishSeconds = CurrentSeconds + delayTask.Seconds;
                    } else if (delayTask.State == DelayTaskState.Started)
                    {
                        if (CurrentSeconds >= delayTask.FinishSeconds)
                        {
                            delayTask.State = DelayTaskState.Finish;
                            delayTask.OnFinish();
                            delayTask.OnFinish = null;
                            mDelayTasks.Remove(currentNode);//删除节点
                        }
                    }
                    currentNode = nextNode;

                }
            }
        }
        public float CurrentSeconds { get; private set; }

        //链表存储延时任务
        private LinkedList<DelayTask> mDelayTasks = new LinkedList<DelayTask>();
        
        public void AddDelayTask(float seconds, Action onDelayFinish)
        {
            var delayTask = new DelayTask()
            {
                Seconds = seconds,
                OnFinish = onDelayFinish,
                State = DelayTaskState.NotStart
            };
            mDelayTasks.AddLast(delayTask);
        }
    }
    
    
}

