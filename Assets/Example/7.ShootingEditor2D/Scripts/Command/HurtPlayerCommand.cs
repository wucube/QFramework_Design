using QFramework;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public class HurtPlayerCommand:AbstractCommand
    {
        //敌人伤害值
        private readonly int mHurt;

        //在构造中设置敌人伤害默认值
        public HurtPlayerCommand(int hurt =1)
        {
            mHurt = hurt;
        }
        protected override void OnExecute()
        {
            var playerModel = this.GetModel<IPlayerModel>();
            playerModel.HP.Value -= mHurt;
            if (playerModel.HP.Value <= 0)
                SceneManager.LoadScene("GameOver");
        }
    }
}