using UnityEngine;

public class GameplayState_Loose : IGameplayState
{
    public void Enter()
    {
        Debug.Log("loose");
    }

    public void Exit()
    {

    }
}
