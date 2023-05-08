using System;
using UnityEngine;

namespace QFramework
{
    public class BindableProperty<T>
    {
        private T mValue = default(T);
        public BindableProperty(T defaultValue = default)
        {
            mValue = defaultValue;
        }
        public T Value
        {
            get =>mValue; 
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && value.Equals(mValue)) return;
                
                mValue = value;
                mOnValueChanged?.Invoke(mValue);
            }
        }
        
        
        private Action<T> mOnValueChanged = (v) => { };

        //值改变时注册委托
        public IUnRegister Register(Action<T> onValueChanged)
        {
            mOnValueChanged += onValueChanged;
            return new BindablePropertyUnRegister<T>() { BindableProperty = this, OnValueChanged = onValueChanged };
        }
        //根据初始值注册
        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(mValue);
            return Register(onValueChanged);
        }
        //值改变时注销委托
        public void UnRegister(Action<T> onValueChanged)
        {
            mOnValueChanged -= onValueChanged;
        }
        
        //比较 操作符   a.Value == b.Value 可直接写作 a==b
        public static implicit  operator T(BindableProperty<T> property)
        {
            return property.Value;
        }

        //将Value转为字符串
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class BindablePropertyUnRegister<T>:IUnRegister 
    {
        public BindableProperty<T> BindableProperty { get; set; }
        
        public Action<T> OnValueChanged { get; set; }

        public void UnRegister()
        {
            BindableProperty.UnRegister(OnValueChanged);
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
}

    

