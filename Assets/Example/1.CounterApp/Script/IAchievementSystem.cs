using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling;
using UnityEngine;
using QFramework;

namespace CounterApp
{
    public interface IAchievementSystem:ISystem { }

    public class AchievementSystem:AbstractSystem, IAchievementSystem
    {
        public IArchitecture Architecture { get; set; }
        protected override void OnInit()
        {
            var counterModel = this.GetModel<ICounterModel>();
            var previousCount = counterModel.Count.Value;
            counterModel.Count.Register(newCount =>
            {
                if (previousCount < 10 && newCount >= 10)
                    Debug.Log("解锁点击 10次成就");
                else if (previousCount < 20 && newCount >= 20)
                    Debug.Log("解锁点击 20 次成就");
                previousCount = newCount;
            });
        }
    }
}

