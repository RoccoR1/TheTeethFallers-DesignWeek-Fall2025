using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{
    public Vector3 movementInputVector { get; private set; }
    public bool isMoveMode;
    private PlayerController playerController;
    private Fishing fishing;

    private void Awake()
    {
        isMoveMode = true;
        playerController = GetComponent<PlayerController>();
        fishing = GetComponent<Fishing>();
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
        if (!fishing.isFishMode)
        {
            fishing.StartFishing();
        }
        else
        {
            fishing.StopFishing();
        }
    }
    private void OnOpenLog()
    {
        // Only opens log if the user isn't fishing.
        if (!fishing.isFishMode)
        {
            isMoveMode = !isMoveMode;
            playerController.OpenLog();
        }
    }
}
