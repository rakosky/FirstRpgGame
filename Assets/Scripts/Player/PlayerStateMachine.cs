public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    public PlayerStateMachine()
    {
    }

    public void Initialize(PlayerState initialState)
    {
        currentState = initialState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState state)
    {
        if (state == currentState)
            return;

        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

}
