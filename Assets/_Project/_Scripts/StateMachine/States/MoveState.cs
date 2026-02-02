using UnityEngine;
using UnityEngine.InputSystem;

namespace Neuromorph
{
public class MoveState: GameState
{
    [SerializeField] Puppet _player;
    public override void OnEnter()
    {
        _player.SetCanMove(true);
    }
    public override void OnExit()
    {
        _player.SetCanMove(false);
    }

    public override void OnUpdate()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            _player.ClickToMove();
    }
}
}