using UnityEngine;

public class MoveCommand : ICommand
{
    public Vector3 Direction;

    //public MoveCommand( Vector3 dir ) => direction = dir; 

    public void Execute( PlayerController player )
    {
        player.Move( Direction );
    }
}
