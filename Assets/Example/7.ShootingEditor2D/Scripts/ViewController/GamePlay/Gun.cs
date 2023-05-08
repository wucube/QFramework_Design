using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class Gun : ShootingEditor2DController
    {
        private Bullet mBullet;
        //枪的信息
        private GunInfo mGunInfo;

        private int mMaxBulletCount;
        private void Awake()
        {
            mBullet = transform.Find("Bullet").GetComponent<Bullet>();
            mGunInfo = this.GetSystem<IGunSystem>().CurrentGun;
            mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGunInfo.Name.Value));
        }

        public void Shoot()
        {
            //枪内子弹数大于0 且 枪械为闲置状态 才能开枪
            if (mGunInfo.BulletCountInGun.Value > 0 && mGunInfo.GunState.Value==GunState.Idle)
            {
                //克隆子弹，未设置父节点
                Bullet bullet =  Instantiate(mBullet, mBullet.transform.position, mBullet.transform.rotation);
                //统一子弹缩放值
                bullet.transform.localScale = mBullet.transform.lossyScale;
                //激活子弹，子弹自动发射
                bullet.gameObject.SetActive(true);
                
                this.SendCommand<ShootCommand>(ShootCommand.Single);
            }
        }

        protected void OnDestroy()
        {
            mGunInfo = null;
        }

        public void Reload()
        {
            if (mGunInfo.GunState.Value == GunState.Idle &&
                mGunInfo.BulletCountInGun.Value != mMaxBulletCount &&
                mGunInfo.BulletCountOutGun.Value > 0)
            {
                this.SendCommand<ReloadCommand>();
            }
            
        }
    }
}

