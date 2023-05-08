using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingEditor2D
{
    public class UIGamePass : MonoBehaviour
    {
        //以默认字体为样版
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 80,
            //文字居中
            alignment = TextAnchor.MiddleCenter
        });
        
        //以默认按钮样式为样版
        private readonly Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter
        });

        //绘制UI
        private void OnGUI()
        {
            var labelWidth = 400;//宽
            var labelHeight = 100;//高
            //位于屏幕正中央
            var labelPosition = new Vector2(Screen.width - labelWidth, Screen.height - labelHeight) * 0.5f;
            var labelSize = new Vector2(labelWidth, labelHeight);
            var labelRect = new Rect(labelPosition, labelSize);

            GUI.Label(labelRect,"游戏通关",mLabelStyle.Value);

            var buttonWidth = 200;
            var buttonHeight = 100;
            //以屏幕尺寸为屏幕正中央，再减去偏移值
            var buttonPosition = new Vector2(Screen.width, Screen.height) * 0.5f -
                                 new Vector2(buttonWidth * 0.5f, buttonHeight * 0.5f)+new Vector2(0,150);

            var buttonSize = new Vector2(buttonWidth, buttonHeight);
            var buttonRect = new Rect(buttonPosition, buttonSize);

            if (GUI.Button(buttonRect, "返回首页", mButtonStyle.Value))
            {
                SceneManager.LoadScene("GameStart");
            }
        }
    }

}
