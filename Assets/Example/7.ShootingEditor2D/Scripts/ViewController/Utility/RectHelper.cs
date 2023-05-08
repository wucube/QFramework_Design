using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootingEditor2D
{
    public static class RectHelper
    {
        //居中锚点的Rect
        public static Rect RectForAnchorCenter(float x, float y,float width,float height)
        {
            var finalX = x - width * 0.5f;
            var finalY = y - height * 0.5f;
            return new Rect(finalX,finalY,width,height);
        }
        
        public static Rect RectForAnchorCenter(Vector2 pos,Vector2 size)
        {
            var finalX = pos.x - size.x * 0.5f;
            var finalY = pos.y - size.y * 0.5f;
            return new Rect(finalX,finalY,size.x,size.y);
        }
    }

}
