using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private Player m_player;
    [SerializeField] private UIMenu m_menu;
    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector3>();
        m_player.SetDirection(direction);
    }
    public void OnEscape(InputAction.CallbackContext context)
    {
        var direction = context.action.triggered;
        if(direction) m_menu.ToggleMenu();
    }
}
