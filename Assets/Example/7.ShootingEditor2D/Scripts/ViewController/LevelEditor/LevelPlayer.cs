using System;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelPlayer : MonoBehaviour
    {
        enum State
        {
            Selection,
            Playing
        }
        private State mCurrentState = State.Selection;

        //路径
        private string mLevelFilesFolder;
        private void Awake()
        {
            mLevelFilesFolder = Application.persistentDataPath + "/LevelFiles";
        }

        //解析xml并运行
        private void ParseAndRun(string xml)
        {
            //xml文件转为字符串
            XmlDocument document = new XmlDocument();
            //读取xml字符串
            document.LoadXml(xml); 
            //获取xml文件中的根节点
            XmlNode levelNode = document.SelectSingleNode("Level");
            //遍历根节点下的子节点
            foreach (XmlElement levelItemNode in levelNode.ChildNodes)
            {
                var levelItemName = levelItemNode.Attributes["name"].Value;
                var levelItemX = int.Parse(levelItemNode.Attributes["x"].Value);
                var levelItemY = int.Parse(levelItemNode.Attributes["y"].Value);
                //加载预制体
                var levelItemPrefab= Resources.Load<GameObject>(levelItemName);
                //实例化预制体， 设置父节点
                var levelItemGameObj = Instantiate(levelItemPrefab, transform);
                //设置实例化对象的位置
                levelItemGameObj.transform.position = new Vector3(levelItemX, levelItemY, 0);
                Debug.Log(levelItemName + ":" + "(" + levelItemX + "," + levelItemY + ")");
            }
        }

        private void OnGUI()
        {
            if (mCurrentState == State.Selection)
            {
                //获取所有文件
                var filePaths = Directory.GetFiles(mLevelFilesFolder);
                
                int y = 10;
                foreach (var filePath in filePaths.Where(f=>f.EndsWith("xml")))
                {
                    //得到文件名
                    var fileName = Path.GetFileName(filePath);
                    if (GUI.Button(new Rect(10, y, 100, 40), fileName))
                    {
                        //加载文件
                        var xml = File.ReadAllText(filePath);
                        ParseAndRun(xml);
                        mCurrentState = State.Playing;
                    }
                    y += 50;
                }
                
            }
        }
    }

}
