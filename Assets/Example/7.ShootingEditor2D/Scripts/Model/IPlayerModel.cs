using QFramework;

namespace ShootingEditor2D
{
    public interface IPlayerModel:IModel
    {
        //主角血量
        BindableProperty<int> HP { get; }
    }
    
    public class PlayerModel:AbstractModel,IPlayerModel
    {
        protected override void OnInit()
        {
            
        }

        public BindableProperty<int> HP { get; } = new BindableProperty<int>() { Value = 3 };
    }
}