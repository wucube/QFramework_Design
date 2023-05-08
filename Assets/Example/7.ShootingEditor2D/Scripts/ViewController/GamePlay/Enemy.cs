using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootingEditor2D
{
    public class Enemy : MonoBehaviour
    {
        private Trigger2DCheck mWallCheck;
        private Trigger2DCheck mFallCheck;
        private Trigger2DCheck mGroundCheck;

        private Rigidbody2D mRigidbody2D;

        private void Awake()
        {
            mWallCheck = transform.Find("WallCheck").GetComponent<Trigger2DCheck>();
            mFallCheck = transform.Find("FallCheck").GetComponent<Trigger2DCheck>();
            mGroundCheck = transform.Find("GroundCheck").GetComponent<Trigger2DCheck>();

            mRigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            //敌人x轴的缩放值的正负 也表示角色的朝向
            var scaleX = transform.localScale.x;
            //若敌人落地，没有掉落，没有障碍物，则向前移动
            if (mGroundCheck.Triggered && mFallCheck.Triggered && !mWallCheck.Triggered)
            {
                mRigidbody2D.velocity = new Vector2(scaleX * 10, mRigidbody2D.velocity.y);
            }
            else //否则敌人就转向
            {
                var localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;
            }
        }
    }

}
