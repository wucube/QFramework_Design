
namespace QFramework.Example
{
    public class PointGame:Architecture<PointGame>
    {
        protected override void Init()
        {
            //注册系统
            RegisterSystem<IScoreSystem>(new ScoreSystem());
            RegisterSystem<ICountDownSystem>(new CountDownSystem());
            RegisterSystem<IAchievementSystem>(new AchievementSystem());
   
            RegisterModel<IGameModel>(new GameModel());
            RegisterUtility<IStorage>(new PlayerPrefsStorage());
        
        }
    }

}
