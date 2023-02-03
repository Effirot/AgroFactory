using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private Player m_player;
    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector3>();
        m_player.SetDirection(direction);
    }
}
