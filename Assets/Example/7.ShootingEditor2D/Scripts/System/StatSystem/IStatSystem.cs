using QFramework;

namespace ShootingEditor2D
{
    public interface IStatSystem:ISystem
    {
        //击杀敌人数量
        BindableProperty<int> killCount { get; }
    }
    
    public class StatSystem : AbstractSystem, IStatSystem
    {
        protected override void OnInit()
        {
            
        }

        public BindableProperty<int> killCount { get; } = new BindableProperty<int>() { Value = 0 };
    }
}