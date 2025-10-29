using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{
    public Vector3 movementInputVector { get; private set; }
    public bool isMoveMode;

    private void Awake()
    {
        isMoveMode = true;
    }
    private void OnMove(InputValue inputValue)
    {
        if (isMoveMode)
        {
            movementInputVector = inputValue.Get<Vector3>();
        }
    }
    private void OnChangeMode(InputValue inputValue)
    {
        isMoveMode = !isMoveMode;
    }
}
