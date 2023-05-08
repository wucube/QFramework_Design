using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class ReloadCommand:AbstractCommand
    {
        protected override void OnExecute()
        {
            var currentGun = this.GetSystem<IGunSystem>().CurrentGun;
            var gunConfigItem = this.GetModel<IGunConfigModel>().GetItemByName(currentGun.Name.Value);

            //计算当前枪需要填充的子弹数量
            var needBulletCount = gunConfigItem.BulletMaxCount - currentGun.BulletCountInGun.Value;
            
            if (needBulletCount > 0)
            {
                if (currentGun.BulletCountOutGun.Value > 0)
                {
                    //切换状态
                    currentGun.GunState.Value = GunState.Reload;
                    
                    //延时切回默认状态
                    this.GetSystem<ITimeSystem>().AddDelayTask(gunConfigItem.ReloadSeconds, () =>
                    {
                        currentGun.GunState.Value = GunState.Idle;
                        //如果枪外子弹很充足
                        if (currentGun.BulletCountOutGun.Value >= needBulletCount)
                        {
                            currentGun.BulletCountInGun.Value += needBulletCount;
                            currentGun.BulletCountOutGun.Value -= needBulletCount;
                        }
                        //如果枪外子弹不充足，就全部填弹
                        else
                        {
                            currentGun.BulletCountInGun.Value += currentGun.BulletCountOutGun.Value;
                            currentGun.BulletCountOutGun.Value = 0;
                        }
                        
                        
                    });
                }
            }
        }
    }

}
