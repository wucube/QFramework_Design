using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework.Example
{
    public class GamePassPanel : MonoBehaviour,IController
    {
        private void Start()
        {
            //显示剩余时间
            transform.Find("RemainSecondsText").GetComponent<Text>().text =
                "剩余时间:" + this.GetSystem<ICountDownSystem>().CurrentRemainSeconds + "s";
            
            var gameModel = this.GetModel<IGameModel>();
            //GameModel的分数与最高分显示到屏幕上
            transform.Find("BestScoreText").GetComponent<Text>().text = "最高分数" + gameModel.BestScore.Value;
            transform.Find("ScoreText").GetComponent<Text>().text = "分类:" + gameModel.Score.Value;
        }

        public IArchitecture GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}

