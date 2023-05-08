namespace QFramework.Example
{
    public class MissCommand:AbstractCommand
    {
        protected override void OnExecute()
        {
            var gameModel = this.GetModel<IGameModel>();
            //若有生命值，错误操作由生命值抵消
            if (gameModel.Life.Value > 0)
                gameModel.Life.Value--;
            else
                this.SendEvent<OnMissEvent>();
        }
    }
}