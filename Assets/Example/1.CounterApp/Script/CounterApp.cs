using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace CounterApp
{
    public class CounterApp:Architecture<CounterApp>
    {
        protected override void Init()
        {
            RegisterSystem<IAchievementSystem>(new AchievementSystem());
            RegisterUtility<IStorage>(new PlayerPrefsStorage());
            RegisterModel<ICounterModel>(new CounterModel());
        }
    }
}

