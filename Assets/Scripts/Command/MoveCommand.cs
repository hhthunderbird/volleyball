using UnityEngine;

public class MoveCommand : ICommand
{
    public Vector3 Direction { get; set; }

    public void Execute( PlayerController player )
    {
        player.Move( Direction );
    }
}
