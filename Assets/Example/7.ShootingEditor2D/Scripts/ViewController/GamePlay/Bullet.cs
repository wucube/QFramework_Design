using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class Bullet : ShootingEditor2DController
    {
        private Rigidbody2D mRigidbody2D;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            //获取玩家全局x轴缩放值的符号
            var isRight = Mathf.Sign(transform.lossyScale.x);
            mRigidbody2D.velocity = Vector2.right * 10 * isRight;
            //5秒后销毁自己
            Destroy(gameObject,5);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                //发送KillEnemyCommand命令
                this.SendCommand<KillEnemyCommand>();
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}

