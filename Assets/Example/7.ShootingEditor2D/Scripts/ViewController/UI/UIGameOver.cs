using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public class UIGameOver : MonoBehaviour
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
            var labelWidth = 600;
            var labelHeight = 100;
            var labelSize  = new Vector2(labelWidth, labelHeight);
            var labelPosition = new Vector2(Screen.width, Screen.height) * 0.5f - labelSize * 0.5f;
            var labelRect = new Rect(labelPosition, labelSize);
            
            GUI.Label(labelRect,"游戏结束",mLabelStyle.Value);

            var buttonWidth = 300;
            var buttonHeight = 100;
            var buttonSize = new Vector2(buttonWidth, buttonHeight);
            //向下偏移150
            var buttonPosition = new Vector2(Screen.width, Screen.height) * 0.5f - buttonSize * 0.5f + Vector2.up * 150;
            var buttonRect = new Rect(buttonPosition, buttonSize);

            if (GUI.Button(buttonRect, "回到首页", mButtonStyle.Value))
            {
                SceneManager.LoadScene("GameStart");
            }
        }
    }
}

