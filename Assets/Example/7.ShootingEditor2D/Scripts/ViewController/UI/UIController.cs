using System;
using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class UIController : ShootingEditor2DController
    {
        private IPlayerModel mPlayerModel;
        private IStatSystem mStatSystem;
        private IGunSystem mGunSystem;

        private int mMaxBulletCount;
        private void Awake()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();
            mStatSystem = this.GetSystem<IStatSystem>();
            mGunSystem = this.GetSystem<IGunSystem>();
            
            //查询数据，显示弹匣容量。使用SendQuery便于管理
            mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGunSystem.CurrentGun.Name.Value));
            //监听事件，枪械变化时重新查询一次
            this.RegisterEvent<OnCurrentGunChanged>(e =>
            {
                mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGunSystem.CurrentGun.Name.Value));
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        //  GUI字体样式设置    第一次调用时，执行 =右边的初始化操作
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 40
        });
        private void OnGUI()
        {
            //在UI中渲染出玩家生命与击杀次数
            GUI.Label(new Rect(10,10,300,100),$"生命:{mPlayerModel.HP.Value}/3",mLabelStyle.Value);
            GUI.Label(new Rect(10,60,300,100),$"枪内子弹:{mGunSystem.CurrentGun.BulletCountInGun.Value}/{mMaxBulletCount}",mLabelStyle.Value);
            GUI.Label(new Rect(10,110,300,100),$"枪外子弹:{mGunSystem.CurrentGun.BulletCountOutGun.Value}",mLabelStyle.Value);
            GUI.Label(new Rect(10,160,300,100),$"枪械名称:{mGunSystem.CurrentGun.Name.Value}",mLabelStyle.Value);
            GUI.Label(new Rect(10,210,300,100),$"枪械状态:{mGunSystem.CurrentGun.GunState.Value}",mLabelStyle.Value);
            GUI.Label(new Rect(Screen.width-10-300,10,300,100),$"击杀次数:{mStatSystem.killCount.Value}",mLabelStyle.Value);
        }

        private void OnDestroy()
        {
            mPlayerModel = null;
            mStatSystem = null;
            mGunSystem = null;
        }
    }
}

