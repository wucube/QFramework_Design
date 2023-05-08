using System.Collections.Generic;
using System.Linq;
using QFramework;

namespace ShootingEditor2D
{
    public interface IGunSystem:ISystem
    {
        GunInfo CurrentGun { get; }
        Queue<GunInfo> GunInfos { get; }
        void PickGun(string name, int bulletCountInGun, int bulletCountOutGun);
        void ShiftGun();
    }
    
    public class OnCurrentGunChanged
    {
        public string Name { get; set; }
    }

    public class GunSystem : AbstractSystem, IGunSystem
    {
        protected override void OnInit() { }

        public GunInfo CurrentGun { get; } = new GunInfo()
        {
            BulletCountInGun = new BindableProperty<int>(){Value = 3},
            BulletCountOutGun = new BindableProperty<int>(){Value = 1},
            Name = new BindableProperty<string>(){Value = "手枪"},
            GunState = new BindableProperty<GunState>(){Value = GunState.Idle}
        };
        
        private Queue<GunInfo> mGunInfos = new Queue<GunInfo>();
        
        public Queue<GunInfo> GunInfos => mGunInfos;

        public void PickGun(string name, int bulletCountInGun, int bulletCountOutGun)
        {
            //当前枪是同类型
            if (CurrentGun.Name.Value == name)
            {
                CurrentGun.BulletCountOutGun.Value += bulletCountInGun;
                CurrentGun.BulletCountOutGun.Value += bulletCountOutGun;
            }
            else if(mGunInfos.Any(gunInfo =>gunInfo.Name.Value==name )) //已经拥有这把枪了
            {
                var gunInfo = mGunInfos.First(info => info.Name.Value == name);
                gunInfo.BulletCountOutGun.Value += bulletCountInGun;
                gunInfo.BulletCountOutGun.Value += bulletCountOutGun;
            }
            else //捡到一把新枪
            {
                EnqueueCurrentGun(name, bulletCountInGun, bulletCountOutGun);
            }
        }

        public void ShiftGun()
        {
            if (mGunInfos.Count > 0)
            {  
                //获取上把枪的信息
                var previousGun = mGunInfos.Dequeue();
                
                EnqueueCurrentGun(previousGun.Name.Value, previousGun.BulletCountInGun.Value, previousGun.BulletCountOutGun.Value);
            }
        }

        //当前枪入队
        void EnqueueCurrentGun(string nextGunName, int nextBulletCountInGun, int nextBulletCountOutGun)
        {
            var currentGunInfo = new GunInfo
            {
                Name = new BindableProperty<string>() { Value = CurrentGun.Name.Value },
                BulletCountInGun = new BindableProperty<int>() { Value = CurrentGun.BulletCountInGun.Value },
                BulletCountOutGun = new BindableProperty<int>() { Value = CurrentGun.BulletCountOutGun.Value},
                GunState = new BindableProperty<GunState>(){Value = CurrentGun.GunState.Value}
            };
            //将复制的当前枪信息入队
            mGunInfos.Enqueue(currentGunInfo);
            //下把枪的信息设置为当前枪的信息
            CurrentGun.Name.Value = nextGunName;
            CurrentGun.BulletCountInGun.Value = nextBulletCountInGun;
            CurrentGun.BulletCountOutGun.Value = nextBulletCountOutGun;
                
            //发送事件通知表现层或其他层抢枪了
            this.SendEvent(new OnCurrentGunChanged(){Name = nextGunName});
        }
    }
    
}