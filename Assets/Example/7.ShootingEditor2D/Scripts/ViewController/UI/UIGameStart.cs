using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public class UIGameStart : MonoBehaviour
    {
        //设置字体样式
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 60,
            alignment = TextAnchor.MiddleCenter //居中
        });
        //设置按钮样式
        private readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter
        });

        private void OnGUI()
        {
            var labelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, Screen.height * 0.5f, 600, 100);
            
            GUI.Label(labelRect,"ShootingEditor2D",mLabelStyle.Value);
            
            //向下偏移150
            var buttonRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, Screen.height * 0.5f + 150, 300, 100);
                
            if (GUI.Button(buttonRect, "开始游戏", mButtonStyle.Value))
            {
                SceneManager.LoadScene("GamePlay");
            }
        }
    }

}

