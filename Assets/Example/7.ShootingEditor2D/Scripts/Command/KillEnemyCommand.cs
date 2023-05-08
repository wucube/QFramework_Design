using System;
using QFramework;
using Random = UnityEngine.Random;

namespace ShootingEditor2D
{
    public class KillEnemyCommand:AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IStatSystem>().killCount.Value++;
            
            var randomIndex = Random.Range(0,100);
            if (randomIndex < 80)
                this.GetSystem<IGunSystem>().CurrentGun.BulletCountInGun.Value += Random.Range(1, 4);

        }
    }
}