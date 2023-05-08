using System;
using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class AttackPlayer:ShootingEditor2DController
    {
        //敌人攻击值
        public int hurt = 1;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.CompareTag("Player"))
                this.SendCommand(new HurtPlayerCommand(hurt));
        }
    }
}