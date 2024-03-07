namespace Game.Gameplay.State
{
    public abstract class AState : IState
    {
        protected EntityController entityController = null;
        protected bool isPhysicsState = false;

        public virtual void Enter(EntityController entityController)
        {
            this.entityController = entityController;
        }

        public virtual IState Execute()
        {
            return null;
        }

        public virtual void Exit() { }
    }
}