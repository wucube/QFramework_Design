using System;

namespace QFramework.Example
{
    public interface ICountDownSystem:ISystem
    {
        int CurrentRemainSeconds { get; }
        void Update();
    }

    public class CountDownSystem : AbstractSystem, ICountDownSystem
    {
        protected override void OnInit()
        {
            this.RegisterEvent<GameStartEvent>(e =>
            {
                //改变状态
                mStarted = true;
                //记录时间
                mGameStartTime = DateTime.Now;
            });
            this.RegisterEvent<GamePassEvent>(e => mStarted = false);
        }
        
        private  DateTime mGameStartTime { get; set; }
        private bool mStarted = false;
        public int CurrentRemainSeconds => 10 - (int)(DateTime.Now - mGameStartTime).TotalSeconds;

        public void Update()
        {
            if(mStarted)
                //记时结束后
                if (DateTime.Now - mGameStartTime > TimeSpan.FromSeconds(10))
                {
                    //发送事件结算结束时间
                    this.SendEvent<OnCountDownEndEvent>();
                    mStarted = false;
                }
        }
    }
}