using UnityEngine;

namespace Game.Gameplay.State
{
    public class StateMachine : MonoBehaviour
    {
        private EntityController entityController = null;
        private IState currentState = null;

        private void Update()
        {
            if (currentState == null) return;

            ExecuteState();
        }

        public void Initialize(EntityController entityController, IState initialState)
        {
            this.entityController = entityController;

            if (initialState == null) return;

            currentState = initialState;
            currentState.Enter(entityController);
        }

        private void ExecuteState()
        {
            if (currentState == null) return;

            IState state = currentState.Execute();
            if (state != null && state != currentState)
            {
                currentState.Exit();
                currentState = state;
                currentState.Enter(entityController);
            }
        }
    }
}