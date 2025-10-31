using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    // This will control if the turntable moves the player forwards or rotates their boat.
    //public static bool isTurntableRotationMoveSideways;

    public Camera playerCamera;
    public GameObject fishLogCanvas;
    public Transform playerPos;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;

    private PlayerInputController inputController;
    
    private void Awake()
    {
        inputController = GetComponent<PlayerInputController>();
        /*if (isTurntableRotationMoveSideways)
        {
            rotateSpeed = 20;
            moveSpeed = 0.1f;
        }
        else
        {
            rotateSpeed = 100;
            moveSpeed = 10;
        }
        Debug.Log(isTurntableRotationMoveSideways);*/
    }

    void Update()
    {
        //music.Play();

        // Keeps camera aligned with boat.
        playerCamera.gameObject.GetComponent<Transform>().position = new Vector3(playerPos.position.x, playerPos.position.y + 1.75f, playerPos.position.z);
        playerCamera.gameObject.GetComponent<Transform>().rotation = playerPos.rotation;
        
        // This will ensure players will not move unless they are in the proper mode. whether it pulls
        if (inputController.isMoveMode/* && isTurntableRotationMoveSideways*/)
        {
            Vector3 positionChange = new Vector3(inputController.movementInputVector.z * moveSpeed * transform.forward.x, 0, inputController.movementInputVector.z * moveSpeed * transform.forward.z);
            transform.position += positionChange * Time.deltaTime;

            Vector3 rotationChange = new Vector3(0, inputController.movementInputVector.x * rotateSpeed, 0);
            transform.Rotate(rotationChange * Time.deltaTime);
        }
        /*else if (inputController.isMoveMode && isTurntableRotationMoveSideways == false)
        {
            Vector3 positionChange = new Vector3(inputController.movementInputVector.x * moveSpeed * transform.forward.x, 0, inputController.movementInputVector.z * moveSpeed * transform.forward.z);
            transform.position += positionChange * Time.deltaTime;

            Vector3 rotationChange = new Vector3(0, inputController.movementInputVector.z * rotateSpeed, 0);
            transform.Rotate(rotationChange * Time.deltaTime);
        }*/
    }

    public void OpenLog()
    {
        fishLogCanvas.SetActive(!fishLogCanvas.activeSelf);
    }
}
