using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject fishLogCanvas;
    public Transform playerPos;

    // velocity things
    float rotVel; // rotation velocity
    float rotAcc; // rotation acceleration
    public float max_rot_speed; // max rotation speed
    public float acc_time; // acceleration time

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;

    private PlayerInputController inputController;
    
    private void Awake()
    {
        inputController = GetComponent<PlayerInputController>();
    }

    void Update()
    {
        // Keeps camera aligned with boat.
        playerCamera.gameObject.GetComponent<Transform>().position = new Vector3(playerPos.position.x, playerPos.position.y + 1.75f, playerPos.position.z);
        playerCamera.gameObject.GetComponent<Transform>().rotation = playerPos.rotation;
        
        // This will ensure players will not move unless they are in the proper mode.
        if (inputController.isMoveMode)
        {
            Vector3 positionChange = new Vector3(inputController.movementInputVector.z * moveSpeed * transform.forward.x, 0, inputController.movementInputVector.z * moveSpeed * transform.forward.z);
            transform.position += positionChange;

            // acc = max-speed/acc-time
            rotAcc = max_rot_speed / acc_time;

            //Vector3 rotationChange = new Vector3(0, inputController.movementInputVector.x * rotateSpeed, 0);
            
            if (inputController.movementInputVector.x > 0)
            {
                rotVel += rotAcc * Time.deltaTime;
            }
            if (inputController.movementInputVector.x < 0)
            {
                rotVel -= rotAcc * Time.deltaTime;
            }
            //rotVel = inputController.movementInputVector.x;
            //transform.Rotate(rotationChange);
            Vector3 rotationChange = new Vector3(0, 0, 0);
            rotationChange.y += rotVel;
            //SwordTransform.Rotate(0, 0, Svel * Time.deltaTime);
            transform.Rotate(0, 0, rotVel, Space.Self);
        }
    }

    public void OpenLog()
    {
        fishLogCanvas.SetActive(!fishLogCanvas.activeSelf);
    }
}
