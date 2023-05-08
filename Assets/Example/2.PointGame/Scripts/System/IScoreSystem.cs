using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example
{
    public interface IScoreSystem:ISystem {  }

    public class ScoreSystem : AbstractSystem, IScoreSystem
    {
        protected override void OnInit()
        {
            var gameModel = this.GetModel<IGameModel>();
            this.RegisterEvent<GamePassEvent>(e =>
            {
                //获取倒计时系统
                var countDownSystem = this.GetSystem<ICountDownSystem>();
                //获取剩余秒数的分数
                var timeScore = countDownSystem.CurrentRemainSeconds * 10;
                //加到分数中
                gameModel.Score.Value += timeScore;

                if (gameModel.Score.Value > gameModel.BestScore.Value)
                {
                    gameModel.BestScore.Value = gameModel.Score.Value;
                    Debug.Log("新纪录");
                }
            });

            this.RegisterEvent<OnEnemyKillEvent>(e =>
            {
                gameModel.Score.Value += 10;
                Debug.Log("得分:10");
                Debug.Log("当前分数:"+gameModel.Score.Value);
            });
            this.RegisterEvent<OnMissEvent>(e =>
            {
                gameModel.Score.Value -= 5;
                Debug.Log("得分:-5");
                Debug.Log("当前分数:"+gameModel.Score.Value);
            });
        }
    }

}
