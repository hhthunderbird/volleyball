using UnityEngine;

public class MoveCommand : ICommand
{
    private Vector3 direction;

    public MoveCommand( Vector3 dir ) => direction = dir; 

    public void Execute( PlayerController player )
    {
        player.Move( direction );
    }
}
