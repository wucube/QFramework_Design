using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class ShootCommand:AbstractCommand
    {
        //避免每次发送命令都new一次
        public static readonly ShootCommand Single = new ShootCommand();
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            gunSystem.CurrentGun.BulletCountInGun.Value--;
            gunSystem.CurrentGun.GunState.Value = GunState.Shooting;
            

            var gunConfigItem = this.GetModel<IGunConfigModel>().GetItemByName(gunSystem.CurrentGun.Name.Value);
            
            //过段时间后枪械状态回归默认状态 Frequency是开枪频率，被1除才转化为时间
            this.GetSystem<ITimeSystem>().AddDelayTask(1 / gunConfigItem.Frequency, () =>
            {
                gunSystem.CurrentGun.GunState.Value = GunState.Idle;
                
                if (gunSystem.CurrentGun.BulletCountInGun.Value == 0 &&
                    gunSystem.CurrentGun.BulletCountOutGun.Value > 0)
                {
                    this.SendCommand<ReloadCommand>();
                }
                
            });
            
        }
    }
}