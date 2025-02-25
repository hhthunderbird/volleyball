public class JumpCommand : ICommand
{
    public void Execute( PlayerController player )
    {
        player.Jump();
    }
}