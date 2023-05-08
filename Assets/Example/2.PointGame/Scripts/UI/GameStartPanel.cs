
using UnityEngine;
using UnityEngine.UI;

namespace QFramework.Example
{
    public class GameStartPanel : MonoBehaviour, IController
    {
        private IGameModel mGameModel;
        void Start()
        {
            transform.Find("BtnStart").GetComponent<Button>().onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                this.SendCommand<StartGameCommand>();
            });

            transform.Find("BtnBuyLife").GetComponent<Button>().onClick.AddListener(() =>
            {
                this.SendCommand<BuyLifeCommand>();
            });

            mGameModel = this.GetModel<IGameModel>();
            mGameModel.Gold.Register(OnGoldValueChanged);
            mGameModel.Life.Register(OnLifeValueChanged);

            //将金币、生命显示到屏幕上，第一次需要调用一下
            OnGoldValueChanged(mGameModel.Gold.Value);
            OnLifeValueChanged(mGameModel.Life.Value);
        }

        private void OnLifeValueChanged(int life)
        {
            transform.Find("LifeText").GetComponent<Text>().text = "生命:" + life;
        }

        private void OnGoldValueChanged(int gold)
        {
            if (gold > 0) transform.Find("BtnBuyLife").gameObject.SetActive(true);

            else transform.Find("BtnBuyLife").gameObject.SetActive(false);

            transform.Find("GoldText").GetComponent<Text>().text = "金币:" + gold;
        }

        private void OnDestroy()
        {
            mGameModel.Gold.UnRegister(OnGoldValueChanged);
            mGameModel.Life.UnRegister(OnLifeValueChanged);
            mGameModel = null;
        }

        IArchitecture ICanGetArchitecture.GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}

