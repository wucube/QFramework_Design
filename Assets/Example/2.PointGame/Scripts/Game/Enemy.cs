using UnityEngine;

namespace QFramework.Example
{
    public class Enemy : MonoBehaviour,IController
    {
        void OnMouseDown()
        {
            gameObject.SetActive(false);
            this.SendCommand<KillEnemyCommand>();
        }
        IArchitecture ICanGetArchitecture.GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}
