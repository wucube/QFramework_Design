using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  ShootingEditor2D
{
    public class Trigger2DCheck : MonoBehaviour
    {
        //目标层级
        public LayerMask TargetLayers;
        //1为落地，0为未落地
        public int EnterCount;
        //检测是否落地
        public bool Triggered => EnterCount > 0;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsInLayerMask(other.gameObject, TargetLayers)) EnterCount++;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsInLayerMask(other.gameObject, TargetLayers)) EnterCount--;
        }

        //判断碰撞的层级是否包含目标对象的层级
        bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            //根据Layer数值的移位获取用于运算的Mask值
            int objLayerMask = 1 << obj.layer;
            return (mask.value & objLayerMask) > 0;
        }
    }
}

