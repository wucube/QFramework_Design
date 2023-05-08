
using UnityEngine;

namespace QFramework.Example
{
    public class UI:MonoBehaviour,IController
    {
        private void Start()
        {
            this.RegisterEvent<GamePassEvent>(OnGamePass);
            this.RegisterEvent<OnCountDownEndEvent>(e =>
            {
                transform.Find("Canvas/GamePanel").gameObject.SetActive(false);
                transform.Find("Canvas/GameOverPanel").gameObject.SetActive(true);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnDestroy()
        {
            this.UnRegisterEvent<GamePassEvent>(OnGamePass);
        }

        private void OnGamePass(GamePassEvent e)
        {
            transform.Find("Canvas/GamePanel").gameObject.SetActive(false);
            transform.Find("Canvas/GamePassPanel").gameObject.SetActive(true);
        }

        public IArchitecture GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}

