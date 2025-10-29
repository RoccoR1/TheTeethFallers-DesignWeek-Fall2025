using UnityEngine;

public class Fishing : MonoBehaviour
{
    public float scratchTimer;
    public bool hasScratchPrompted;

    private PlayerController controller;
    private PlayerInputController inputController;
    
    void Awake()
    {
        controller = GetComponent<PlayerController>();
        inputController = GetComponent<PlayerInputController>();
        scratchTimer = Random.Range(5, 10);
        hasScratchPrompted = false;
    }

    void Update()
    {
        if (!inputController.isMoveMode)
        {
            scratchTimer -= Time.deltaTime;
        }
        if (scratchTimer <= 3 && !hasScratchPrompted)
        {
            hasScratchPrompted = true;
        }
        if (scratchTimer <= 0)
        {
            scratchTimer = Random.Range(5, 10);
            hasScratchPrompted = false;
        }
    }
}
