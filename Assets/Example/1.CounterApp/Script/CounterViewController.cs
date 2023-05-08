using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace CounterApp
{
    public class CounterViewController : MonoBehaviour, IController
    {
        private ICounterModel mCounterModel;
        void Start()
        {
            mCounterModel = this.GetModel<ICounterModel>();
    
            mCounterModel.Count.Register(OnCountChanged);
    
            transform.Find("BtnAdd").GetComponent<Button>()
                .onClick.AddListener(() => { 
                    //交互逻辑，会自动触发表现逻辑
                   this.SendCommand<AddCountCommand>();
                });
    
            transform.Find("BtnSub").GetComponent<Button>()
                .onClick.AddListener(() => {
                    //交互逻辑，会自动触发表现逻辑
                    this.SendCommand<SubCountCommand>();
                });
            //开始游戏后主动调用一次
            OnCountChanged(mCounterModel.Count.Value);
        }
    
        private void OnCountChanged(int newCount)
        {
            transform.Find("CountText").GetComponent<Text>().text = newCount.ToString();
        }
    
        private void OnDestroy()
        {
            //注销委托
            mCounterModel.Count.UnRegister(OnCountChanged);
            //Container置空
            mCounterModel = null;
        }
    
        //public IArchitecture Architecture { get; set; } = CounterApp.Interface;
    
        IArchitecture  ICanGetArchitecture.GetArchitecture()
        {
            return CounterApp.Interface;
        }
        
    }
    
    public interface ICounterModel : IModel
    {
        BindableProperty<int> Count { get; }
    }
    
    
    //计数器的数据类
    public  class CounterModel:AbstractModel,ICounterModel
    {
        protected override void OnInit()
        {
            var storage = this.GetUtility<IStorage>();
            
            Count.Value = storage.LoadInt("COUNTER_COUNT", 0);
            Count.Register(count=> { storage.SaveInt("COUNTER_COUNT", count); }); 
        }
    
        public BindableProperty<int> Count { get; } = new BindableProperty<int>()
        {
            Value = 0
        };
    
        //public IArchitecture Architecture { get; set; }
    }

}
