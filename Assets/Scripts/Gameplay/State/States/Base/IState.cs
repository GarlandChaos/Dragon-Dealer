namespace Game.Gameplay.State
{
    public interface IState
    {
        public void Enter(EntityController entityController);
        public IState Execute();
        public void Exit();
    }
}