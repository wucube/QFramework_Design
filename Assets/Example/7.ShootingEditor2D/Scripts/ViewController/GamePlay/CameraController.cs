using System;
using UnityEngine;
namespace ShootingEditor2D
{
    public class CameraController:MonoBehaviour
    {
        private Transform mPlayerTrans;

        //摄像机移动边距范围
        private float xMin = -5;
        private float xMax = 5;
        private float yMin = -5;
        private float yMax = 5;

        //目标位置点
        private Vector3 mTargetPos;
        private void LateUpdate()
        {
            //获取玩家的Transform
            if (!mPlayerTrans)
            {
                GameObject playerGameObj = GameObject.FindWithTag("Player");
                if (playerGameObj) mPlayerTrans = playerGameObj.transform;
                else return;
            }
            //玩家位置加上偏移值后，赋值给Camera的位置属性
            Vector3 cameraPos = transform.position;
            //获取数值的符号(-1，0，1)
            var isRight = Mathf.Sign(mPlayerTrans.transform.localScale.x);
            
            Vector3 playerPos = mPlayerTrans.transform.position;

            mTargetPos.x = playerPos.x + 3 * isRight;
            mTargetPos.y = playerPos.y + 2;
            mTargetPos.z = -10;
            //平滑速度
            var smoothSpeed = 5;
            //摄像机平滑移动
            var position = transform.position;
            position = Vector3.Lerp(position, mTargetPos, smoothSpeed * Time.deltaTime);
            //摄像机在固定区域
            transform.position = new Vector3(Mathf.Clamp(position.x, xMin, xMax), Mathf.Clamp(position.y, yMin, yMax),position.z);

            
        }
    }
}