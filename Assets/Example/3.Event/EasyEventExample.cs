using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example
{
    public class EasyEventExample : MonoBehaviour
    {
        private EasyEvent mOnMouseLeftClickEvent = new EasyEvent();                  
    
        private EasyEvent<int> mOnValueChanged = new EasyEvent<int>();                  
    
        public class EventA : EasyEvent<int,int> { }          
    
        private EventA mEventA = new EventA();          
    
        private void Start()         
        {             
            mOnMouseLeftClickEvent.Register(() =>             
                {                 
                    Debug.Log("鼠标左键点击");             
                })
                .UnRegisterWhenGameObjectDestroyed(gameObject);              
      
            mOnValueChanged.Register(value =>             
                {                  
                    Debug.Log($"值变更:{value}");             
                })
                .UnRegisterWhenGameObjectDestroyed(gameObject);               
      
            mEventA.Register((a, b) =>             
                {                 
                    Debug.Log($"自定义事件:{a} {b}");             
                })
                .UnRegisterWhenGameObjectDestroyed(gameObject);         
        }          
    
        private void Update()         
        {             
            if (Input.GetMouseButtonDown(0))             
            {                 
                mOnMouseLeftClickEvent.Trigger();             
            }                          
      
            if (Input.GetMouseButtonDown(1))             
            {                 
                mOnValueChanged.Trigger(10);             
            }              
      
            // 鼠标中键 
            if (Input.GetMouseButtonDown(2))             
            {                 
                mEventA.Trigger(1,2);             
            }         
        }  
        
        // 输出结果： 
// 按鼠标左键时，输出: // 鼠标左键点击 // 按鼠标右键时，输出: // 值变更:10 // 按鼠标中键时，输出: // 自定义事件:1 2 
    }
}

