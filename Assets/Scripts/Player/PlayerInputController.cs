using UnityEngine;
using UnityEngine.InputSystem;

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

    private void OnMove(InputValue inputValue)
    {
        Vector3 rawInput = inputValue.Get<Vector3>();

        // Deadzone: If input is tiny, just make it zero to avoid "ghost" sounds
        if (rawInput.magnitude < 0.1f) rawInput = Vector3.zero;

        movementInputVector = rawInput;

        if (isMoveMode)
        {
            if (turntable != null)
            {
                turntable.transform.Rotate(0, 0, movementInputVector.z * Time.deltaTime * 50);
            }

            // This sends the data to the threshold check
            playerController.AttemptRowing(movementInputVector.z);
        }
    }

    private void OnChangeMode(InputValue inputValue)
    {
        isMoveMode = !isMoveMode;
        if (fishing != null)
        {
            if (!fishing.CheckIfFishing()) fishing.StartFishing();
            else fishing.StopFishing();
        }
    }

    private void OnOpenLog()
    {
        if (fishing != null && !fishing.CheckIfFishing())
        {
            isMoveMode = !isMoveMode;
            playerController.OpenLog();
        }
    }
}