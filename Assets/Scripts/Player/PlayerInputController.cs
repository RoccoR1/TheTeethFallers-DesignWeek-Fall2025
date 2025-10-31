using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{
    public GameObject turntable;
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
    
    // Each On____ method corresponds to a input Action.
    private void OnMove(InputValue inputValue)
    {
        movementInputVector = inputValue.Get<Vector3>();
        turntable.transform.Rotate(0,0,turntable.transform.rotation.z + movementInputVector.z * Time.deltaTime * 50);
    }

    // Calls for the inputs assigned to the Action ChangeMode
    private void OnChangeMode(InputValue inputValue)
    {
        isMoveMode = !isMoveMode;
        if (fishing.CheckIfFishing() == false)
        {
            fishing.StartFishing();
        }
        else
        {
            fishing.StopFishing();
        }
    }

    // Opens the log. Activates when input assigned to OpenLog input action is entered.
    private void OnOpenLog()
    {
        // Only opens log if the user isn't fishing.
        if (fishing.CheckIfFishing() == false)
        {
            isMoveMode = !isMoveMode;
            playerController.OpenLog();
        }
    }
}
