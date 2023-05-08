namespace QFramework.Example
{
    public class KillEnemyCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var gameModel = this.GetModel<IGameModel>();
            gameModel.KillCount.Value++;

            //每击杀一名敌人随机获取1~3枚金币
            if (UnityEngine.Random.Range(0,10) < 3)
                gameModel.Gold.Value += UnityEngine.Random.Range(1, 3);
            //发送事件
            this.SendEvent<OnEnemyKillEvent>();
        
            if (gameModel.KillCount.Value == 10)
            {
                this.SendEvent<GamePassEvent>();
            }
            
        }
    }
}

