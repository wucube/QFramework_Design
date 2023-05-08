using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework.Example
{
    public class GamePanel : MonoBehaviour,IController
    {
        private ICountDownSystem mCountDownSystem;
        private IGameModel mGameModel;

        private void Awake()
        {
            mCountDownSystem = this.GetSystem<ICountDownSystem>();
            mGameModel = this.GetModel<IGameModel>();

            mGameModel.Gold.Register(OnGoldValueChanged);
            mGameModel.Life.Register(OnLifeValueChanged);
            mGameModel.Score.Register(OnScoreValueChanged);

            OnGoldValueChanged(mGameModel.Gold.Value);
            OnLifeValueChanged(mGameModel.Life.Value);
        }

        private void OnScoreValueChanged(int gold)
        {
            transform.Find("GoldText").GetComponent<Text>().text = "金币:" + gold;
        }

        private void OnLifeValueChanged(int life)
        {
            transform.Find("LifeText").GetComponent<Text>().text = "生命:" + life;
        }

        private void OnGoldValueChanged(int score)
        {
            transform.Find("ScoreText").GetComponent<Text>().text = "分数:" + score;
        }
        void Update()
        {
            //每20帧更新一次
            if (Time.frameCount % 20 == 0)
            {
                transform.Find("CountDownText").GetComponent<Text>().text = mCountDownSystem.CurrentRemainSeconds + "s";
                mCountDownSystem.Update();
            }
        }

        private void OnDestroy()
        {
            mGameModel.Gold.UnRegister(OnGoldValueChanged);
            mGameModel.Life.UnRegister(OnLifeValueChanged);
            mGameModel.Score.UnRegister(OnScoreValueChanged);
            mGameModel = null;
            mCountDownSystem = null;
        }

        public IArchitecture GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}

