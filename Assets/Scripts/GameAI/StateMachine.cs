

//modified by A. D. Hartl, Carinthia University of Applied Sciences, 05.05.2021


namespace RayWenderlich.Unity.StatePatternInUnity
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
        }

        public void ChangeState(State newState)
        {
            CurrentState.Exit();

            CurrentState = newState;
            newState.Enter();
        }
    }
}
