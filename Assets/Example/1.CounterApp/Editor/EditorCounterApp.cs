using QFramework;
using UnityEditor;
using UnityEngine;

namespace CounterApp
{
    public class EditorCounterApp : EditorWindow,IController
    {
        [MenuItem("EditorCounterApp/Open")]
        //打开窗口
        static void Open()
        {
            CounterApp.OnRegisterPatch += app => app.RegisterUtility<IStorage>(new EditorPrefsStorage());
        
            var window = GetWindow<EditorCounterApp>();
            window.position = new Rect(100, 100, 400, 600);
            window.titleContent = new GUIContent(nameof(EditorCounterApp));
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("+"))
            {
                this.SendCommand<AddCountCommand>();
            }
        
            //渲染字符
            GUILayout.Label(CounterApp.Interface.GetModel<ICounterModel>().Count.Value.ToString());

            if (GUILayout.Button("-"))
            {
                this.SendCommand<SubCountCommand>();
            }
        }

        IArchitecture ICanGetArchitecture.GetArchitecture()
        {
            return CounterApp.Interface;
        }
    }
}

