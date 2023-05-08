using UnityEngine;

namespace QFramework.Example
{
    public class Game:MonoBehaviour,IController
    {
        private IController _controllerImplementation;

        void Start()
        {
            this.RegisterEvent<GameStartEvent>(OnGameStart);
            this.RegisterEvent<OnCountDownEndEvent>(e => { transform.Find("Enemies").gameObject.SetActive(false);})
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<GamePassEvent>(e=>{transform.Find("Enemies").gameObject.SetActive(false);})
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        
        }
        void OnDestroy()
        {
            this.UnRegisterEvent<GameStartEvent>(OnGameStart);
        }
        void OnGameStart(GameStartEvent e)
        {
            //将敌人子节点显示出来
            var enemyRoot = transform.Find("Enemies");
            enemyRoot.gameObject.SetActive(true);
            foreach (Transform childTrans in enemyRoot)
            {
                childTrans.gameObject.SetActive(true);
            }
        }

        public IArchitecture GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}
