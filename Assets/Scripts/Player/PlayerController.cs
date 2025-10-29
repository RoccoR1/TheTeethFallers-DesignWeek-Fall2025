using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public Transform playerPos;
    public bool isFishing;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;

    private PlayerInputController inputController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        inputController = GetComponent<PlayerInputController>();
        isFishing = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.gameObject.GetComponent<Transform>().position = new Vector3(playerPos.position.x, playerPos.position.y + 1.75f, playerPos.position.z);
        playerCamera.gameObject.GetComponent<Transform>().rotation = playerPos.rotation;

        Vector3 positionChange = new Vector3(inputController.movementInputVector.z * moveSpeed * transform.forward.x, 0, inputController.movementInputVector.z*moveSpeed*transform.forward.z);
        transform.position += positionChange;

        Vector3 rotationChange = new Vector3(0, inputController.movementInputVector.x * rotateSpeed, 0);
        //transform.rotation += rotationChange;
        //Quaternion changeRotation = Quaternion.Euler(0,inputController.movementInputVector.y * rotateSpeed,0);
        transform.Rotate(rotationChange);
    }

    public void PlayerMovement()
    {

    }
}
