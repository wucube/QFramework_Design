using QFramework;

namespace ShootingEditor2D
{
    public class AddBulletCommand:AbstractCommand
    {
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            
            //给当前的枪添加子弹
            AddBullet(gunSystem.CurrentGun,gunConfigModel);
            //遍历缓存的枪
            foreach (var gunInfo in gunSystem.GunInfos)
            {
                //为枪添加子弹
                AddBullet(gunInfo,gunConfigModel);
            }
        }

        void AddBullet(GunInfo gunInfo,IGunConfigModel gunConfigModel)
        {
            var gunConfigItem = gunConfigModel.GetItemByName(gunInfo.Name.Value);
            //如果是手枪
            if (!gunConfigItem.NeedBullet) 
            {
                //什么都不做
            }
            else gunInfo.BulletCountOutGun.Value += gunConfigItem.BulletMaxCount;
            
        }
    }
}