using UnityEngine;

namespace QFramework.Example
{
    public class IOCExample : MonoBehaviour
    {
        private void Start()
        {
            var container = new IocContainer();
        
            //注册接口的实现
            container.Register<IBluetoothMonager>(new BluetoothManager());

            //根据类型获取接口(实例)
            var bluetoothManager =  container.Get<IBluetoothMonager>();
        
            //使用对象
            bluetoothManager.Connect();
        }
    
        public interface IBluetoothMonager
        {
            void Connect();
        }
        public class BluetoothManager:IBluetoothMonager
        {
            public void Connect()
            {
                Debug.Log("蓝牙链接成功");
            }
        }
    }
}

