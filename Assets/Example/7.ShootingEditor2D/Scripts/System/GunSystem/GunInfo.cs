using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace ShootingEditor2D
{
    public enum GunState
    {
        Idle,
        Shooting,
        Reload,
        EmptyBullet,
        CoolDown
    }
       
    public class GunInfo
    {
    
        //弃用当前字段
        [Obsolete("请使用 BulletCountInGun",true)]
        public BindableProperty<int> BulletCount
        {
            get => BulletCountInGun;
            set => BulletCountInGun = value;
        }

        public BindableProperty<int> BulletCountInGun;

        public BindableProperty<string> Name;
        
        public BindableProperty<GunState> GunState;

        public BindableProperty<int> BulletCountOutGun;
    }
}

