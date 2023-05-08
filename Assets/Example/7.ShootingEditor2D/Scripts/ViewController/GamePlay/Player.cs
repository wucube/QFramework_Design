 using System;
 using System.Collections;
using System.Collections.Generic;
 using QFramework;
 using UnityEngine;

 namespace ShootingEditor2D
 {
     public class Player : ShootingEditor2DController
     {
         private Rigidbody2D mRigidbody2D;

         private Trigger2DCheck mGroundCheck;

         private Gun mGun;

         //是否按下跳跃键
         private bool mJumpPressed;
         private void Awake()
         {
             mRigidbody2D = GetComponent<Rigidbody2D>();
             mGroundCheck = transform.Find("GroundCheck").GetComponent<Trigger2DCheck>();
             mGun = transform.Find("Gun").GetComponent<Gun>();
         }
         private void Update()
         {
             //监听跳跃键是否按下
             if (Input.GetKeyDown(KeyCode.K)) mJumpPressed = true;
             if (Input.GetKeyDown(KeyCode.J)) mGun.Shoot();
             if (Input.GetKeyDown(KeyCode.R)) mGun.Reload();
             if (Input.GetKeyDown(KeyCode.Q)) this.SendCommand<ShiftGunCommand>();
         }
         private void FixedUpdate()
         {
             //获取水平轴输入
             var horizontalMovement = Input.GetAxis("Horizontal");

             //移动方向与朝向相反，就要转向
             if (horizontalMovement > 0 && transform.localScale.x < 0 ||
                 horizontalMovement < 0 && transform.localScale.x > 0)
             {
                 var localScale = transform.localScale;
                 localScale.x = -localScale.x;
                 transform.localScale = localScale;
             }
                 
             //为玩家刚体速度赋值，使用刚体Y轴值，未来可能跳跃
             mRigidbody2D.velocity = new Vector2(horizontalMovement*5, mRigidbody2D.velocity.y);
             
             bool grounded = mGroundCheck.Triggered;
             //为玩家刚体施加向上的力,按键按下并且落地时
             if (mJumpPressed && grounded) mRigidbody2D.velocity = new Vector2(mRigidbody2D.velocity.x, 5);
             mJumpPressed = false;
         }
     }
 }

