using UnityEngine;

namespace QFramework.Example
{
    public class BindablePropertyExample : MonoBehaviour
    {
        private BindableProperty<int> age = new BindableProperty<int>(10);
        private BindableProperty<int> counter = new BindableProperty<int>();

        private void Start()
        {
            age.Register(age =>
            {
                Debug.Log(nameof(age) + ":" + age);
                
                counter.Value = 10 * age;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            counter.RegisterWithInitValue(counter =>
            {
                Debug.Log(nameof(counter) + ":" + counter);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                age.Value++;
            }
        }
    }
}

