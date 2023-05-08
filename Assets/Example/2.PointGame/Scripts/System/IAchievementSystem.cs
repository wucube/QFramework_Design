using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace QFramework.Example
{
    public interface IAchievementSystem:ISystem { }

    //每项成就封装为一个对象
    public class AchievementItem
    {
        //成就名
        public string Name { get; set; }
        //完成条件
        public Func<bool> CheckComplete { get; set; }
        //是否解锁
        public bool Unlocked { get; set; }
    }
    
    public class AchievementSystem:AbstractSystem,IAchievementSystem
    {
        //存储成就对象
        private List<AchievementItem> mItems = new List<AchievementItem>();

        private bool mMissed = false;
        protected override void OnInit()
        {
            this.RegisterEvent<OnMissEvent>(e => mMissed = true);
            this.RegisterEvent<GameStartEvent>(e => mMissed = false);
            
            mItems.Add(new AchievementItem()
            {
                Name = "百分成就",
                CheckComplete = ()=>this.GetModel<IGameModel>().BestScore.Value>100
            });
            mItems.Add(new AchievementItem()
            {
                Name = "手残",
                CheckComplete = ()=>this.GetModel<IGameModel>().BestScore.Value < 0
            });
            mItems.Add(new AchievementItem()
            {
                Name = "零失误成就",
                CheckComplete = ()=>!mMissed
            });
            mItems.Add(new AchievementItem()
            {
                Name = "零失误成就",
                //CheckComplete = ()=> mItems.Count(item=>item.Unlocked)>=3 不知出错原因
                CheckComplete = ()=> mItems.Count>=3
            });
            
            //成就系统一般是持久化的，要持久化就在这个时机进行，让Unlocked变为BindableProperty
            this.RegisterEvent<GamePassEvent>(async e =>
            {
                //等待0.1秒，确保在所有计算后再做
                await Task.Delay(TimeSpan.FromSeconds(0.1f));
                foreach (var achievementItem in mItems)
                {
                    if (!achievementItem.Unlocked && achievementItem.CheckComplete())
                    {
                        achievementItem.Unlocked = true;
                        Debug.Log("解锁 成就:"+achievementItem.Name);
                    }
                }

            });
        }
    }
}