using QFramework;

namespace ShootingEditor2D
{
    public class FullBulletCommand:AbstractCommand
    {
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            var gunConfigModel = this.GetModel<IGunConfigModel>();

            //填满当前枪的弹匣
            gunSystem.CurrentGun.BulletCountInGun.Value =
                gunConfigModel.GetItemByName(gunSystem.CurrentGun.Name.Value).BulletMaxCount;

            //填满所有缓存枪的弹匣
            foreach (var gunInfo in gunSystem.GunInfos)
            {
                gunInfo.BulletCountInGun.Value =
                    gunConfigModel.GetItemByName(gunInfo.Name.Value).BulletMaxCount;
            }
        }
    }
}

