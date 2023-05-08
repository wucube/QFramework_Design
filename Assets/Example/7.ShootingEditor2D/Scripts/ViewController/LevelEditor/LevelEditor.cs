using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

namespace ShootingEditor2D
{
    public class LevelEditor : MonoBehaviour
    {
        //绘制模式枚举
        public enum OperateMode
        {
            Draw,
            Erase
        }
        
        //笔刷枚举
        public enum BrushType
        {
            Ground,
            Player
        }
        //关卡物品的信息
        class LevelItemInfo
        {
            public string name;
            public float x;
            public float y;
        }
        //当前操作模式
        private OperateMode mCurrentOperateMode;
        
        //当前笔刷类型
        private BrushType mCurrentBrushType = BrushType.Ground;
        
        public SpriteRenderer emptyHighlight;
        
        //是否能绘制
        private bool mCanDraw;
        //当前鼠标上的对象
        private GameObject mCurrentObjectMouseOn;
        //设置字体样式
        private Lazy<GUIStyle> mModeLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 30,
            alignment = TextAnchor.MiddleCenter
        });
        private Lazy<GUIStyle> mButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 30
        });

        private Lazy<GUIStyle> mRightButtonStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
        {
            fontSize = 25
        });
        private void OnGUI()
        {
            //当前的绘制模式显示到UI中
            var modeLabelRect = RectHelper.RectForAnchorCenter(Screen.width*0.5f,35,300,50);

            if (mCurrentOperateMode==OperateMode.Draw)//若是绘制模式，就显示当前笔刷类型
            {
                GUI.Label(modeLabelRect,mCurrentOperateMode+":"+mCurrentBrushType,mModeLabelStyle.Value);
            }
            else
            {
                GUI.Label(modeLabelRect,mCurrentOperateMode.ToString(),mModeLabelStyle.Value);
            }
            
            //绘制按钮显示到UI，屏幕左上角为原点
            var drawButtonRect = new Rect(10, 10, 150, 50);
            if (GUI.Button(drawButtonRect, "绘制",mButtonStyle.Value))
            {
                mCurrentOperateMode = OperateMode.Draw;
            }
            //擦除按钮显示到UI
            var eraseButtonRect = new Rect(10, 60, 150, 50);
            if (GUI.Button(eraseButtonRect, "橡皮",mButtonStyle.Value))
            {
                mCurrentOperateMode = OperateMode.Erase;
            }

            if (mCurrentOperateMode == OperateMode.Draw) //若是绘制模式，则显示笔刷按钮
            {
                //地块笔刷按钮显示到UI
                var groundButtonRect = new Rect(Screen.width - 110, 10, 100, 50);
                if(GUI.Button(groundButtonRect,"地块",mButtonStyle.Value))
                {
                    mCurrentBrushType = BrushType.Ground;
                }
                //玩家笔刷按钮显示到UI
                var playerButtonRect = new Rect(Screen.width - 110, 70, 100, 50);
                if (GUI.Button(playerButtonRect, "主角", mRightButtonStyle.Value))
                {
                    mCurrentBrushType = BrushType.Player;
                }
            }
            //保存按键显示到UI
            var saveButtonRect = new Rect(Screen.width - 110, Screen.height - 60, 100, 50);
            if (GUI.Button(saveButtonRect, "保存", mRightButtonStyle.Value))
            {
                Debug.Log("保存");
                var infos = new List<LevelItemInfo>(transform.childCount);
                //遍历父节点下的子节点，保存子节点的信息
                foreach (Transform child in transform)
                {
                    infos.Add(new LevelItemInfo()
                    {
                        name = child.name,
                        x = child.position.x,
                        y = child.position.y
                    });
                    
                    //Debug.Log($"Name:{child.name} X:{child.position.x} Y:{child.position.y}");
                }
                //创建XML类
                var document = new XmlDocument();
                //声明xml的配置
                var declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");
                //添加声明
                document.AppendChild(declaration);
                //创建 节点并添加
                var level = document.CreateElement("Level");
                document.AppendChild(level);
                
                foreach (var levelItemInfo in infos)
                {
                    //创建节点并设置，最后添加到xml中
                    var levelItem = document.CreateElement("LevelItem");
                    levelItem.SetAttribute("name", levelItemInfo.name);
                    levelItem.SetAttribute("x", levelItemInfo.x.ToString());
                    levelItem.SetAttribute("y", levelItemInfo.y.ToString());
                    level.AppendChild(levelItem);
                }
                
                //将xml文件排版后打印到控制台
                // var stringBuilder = new StringBuilder();
                // var stringWriter = new StringWriter(stringBuilder);
                // var xmlWriter = new XmlTextWriter(stringWriter);
                // xmlWriter.Formatting = Formatting.Indented;//缩进
                // document.WriteTo(xmlWriter);
                
                //Debug.Log(stringBuilder.ToString());

                //指定目录
                var levelFilesFolder = Application.persistentDataPath + "/LevelFiles";
                //目录不存在就创建
                if (!Directory.Exists(levelFilesFolder))
                    Directory.CreateDirectory(levelFilesFolder);
                //创建用于存储的文件，根据创建时间名称
                var levelFilePath = levelFilesFolder + "/" + DateTime.Now.ToString("yyyy MMMM dd hh mm ss") + ".xml";
                document.Save(levelFilePath);
            }
        }

        private void Update()
        {
            //获取鼠标坐标
            var mousePosition = Input.mousePosition;
            //鼠标屏幕坐标转为编辑器世界坐标
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
            //四舍五入取整
            mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f);
            mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
            //设置转化后的Z轴
            mouseWorldPos.z = 0;
            
            //若鼠标与GUI存在交互内容，就隐藏高亮块
            if(GUIUtility.hotControl==0)
                emptyHighlight.gameObject.SetActive(true);
            else
                emptyHighlight.gameObject.SetActive(false);

                //志当前高亮块的位置相同
            if (Math.Abs(emptyHighlight.transform.position.x - mouseWorldPos.x) < 0.1f&&
                Math.Abs(emptyHighlight.transform.position.y - mouseWorldPos.y) < 0.1f)
            {
                //不做任何事情
            }
            else//不在当前位置
            {
                var highlightPos = mouseWorldPos;
                highlightPos.z = -9;//更靠近屏幕
                //高亮方块的坐标更靠近屏幕，在上方渲染出来
                emptyHighlight.transform.position = highlightPos;

                //发射一条经过屏幕点的射线
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                var hit = Physics2D.Raycast(ray.origin, Vector2.zero, 20);
                //射线是否碰撞到地块
                if (hit.collider)
                {
                    if(mCurrentOperateMode==OperateMode.Draw)
                        emptyHighlight.color = new Color(1, 0, 0, 0.5f);//红色代表不能绘制
                    
                    else if(mCurrentOperateMode==OperateMode.Erase)
                        emptyHighlight.color = new Color(1, 0.5f, 0, 0.5f);//橙色代表可探险
                    
                    mCanDraw = false;
                    //缓存鼠标下已有的地块
                    mCurrentObjectMouseOn = hit.collider.gameObject;
                }
                else
                {
                    if(mCurrentOperateMode==OperateMode.Draw)
                        emptyHighlight.color = new Color(1, 1, 1, 0.5f);//白色代表可绘制
                    
                    else if(mCurrentOperateMode==OperateMode.Erase) 
                        emptyHighlight.color = new Color(0, 0, 1, 0.5f);//蓝色代表橡皮状态
                    
                    mCanDraw = true;
                    mCurrentObjectMouseOn = null;
                }
            }
            
            if (Input.GetMouseButtonDown(0) ||Input.GetMouseButton(0) && GUIUtility.hotControl==0)
            {
                //能绘制且处于绘画模式
                if (mCanDraw&&mCurrentOperateMode==OperateMode.Draw)
                {
                    if (mCurrentBrushType == BrushType.Ground)
                    {
                        //加载地形资源预制体并实例化
                        var groundPrefab =  Resources.Load<GameObject>("Ground");
                        var groundGameObj = Instantiate(groundPrefab, transform);
                        groundGameObj.transform.position = mouseWorldPos;
                        groundGameObj.name = "Ground";

                        //防止重复绘制
                        mCanDraw = false;
                    }
                    else if(mCurrentBrushType==BrushType.Player)
                    {
                        //加载主角资源预制体并实例化，暂时用地形资源代替
                        var groundPrefab =  Resources.Load<GameObject>("Ground");
                        var groundGameObj = Instantiate(groundPrefab, transform);
                        groundGameObj.transform.position = mouseWorldPos;
                        groundGameObj.name = "Player";
                        groundGameObj.GetComponent<SpriteRenderer>().color = Color.cyan;
                        //防止重复绘制
                        mCanDraw = false;
                    }
                    
                }
                //当前有对象可销毁时
                else if (mCurrentObjectMouseOn && mCurrentOperateMode == OperateMode.Erase)
                {
                    Destroy(mCurrentObjectMouseOn);

                    mCurrentObjectMouseOn = null;
                }
            }
        }
    }

}
