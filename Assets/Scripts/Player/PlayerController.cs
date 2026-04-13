using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject fishLogCanvas;
    public Transform playerPos;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 100f;
    [Range(0, 1)]
    [SerializeField] private float rowingThreshold = 0.5f; // Only play sound if push is this strong

    private PlayerInputController inputController;
    private bool canRow = true;

    private void Awake()
    {
        inputController = GetComponent<PlayerInputController>();
    }

    void Update()
    {
        if (playerPos != null && playerCamera != null)
        {
            playerCamera.transform.position = new Vector3(playerPos.position.x, playerPos.position.y + 1.75f, playerPos.position.z);
            playerCamera.transform.rotation = playerPos.rotation;
        }

        if (inputController.isMoveMode)
        {
            Vector3 positionChange = new Vector3(inputController.movementInputVector.z * moveSpeed * transform.forward.x, 0, inputController.movementInputVector.z * moveSpeed * transform.forward.z);
            transform.position += positionChange * Time.deltaTime;

            Vector3 rotationChange = new Vector3(0, inputController.movementInputVector.x * rotateSpeed, 0);
            transform.Rotate(rotationChange * Time.deltaTime);
        }
    }

    public void AttemptRowing(float inputZ)
    {
        // Now requires a stronger push (Threshold) AND the 3-second cooldown
        if (inputZ > rowingThreshold && canRow)
        {
            StartCoroutine(RowingCooldownSequence());
        }
    }

    IEnumerator RowingCooldownSequence()
    {
        canRow = false;

        // Play the sound
        FMODUnity.RuntimeManager.PlayOneShot("event:/rowing");
        Debug.Log("Rowing stroke performed!");

        // Wait 3 seconds - this ensures a smooth rhythm
        yield return new WaitForSeconds(3f);

        canRow = true;
    }

    public void OpenLog()
    {
        if (fishLogCanvas != null)
            fishLogCanvas.SetActive(!fishLogCanvas.activeSelf);
    }
}