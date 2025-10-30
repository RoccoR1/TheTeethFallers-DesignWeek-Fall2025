using UnityEngine;

public class Fishing : MonoBehaviour
{
    public GameObject fishingRod;
    public GameObject timerRing;
    public GameObject fishingUI;
    
    // Determines which fish is currently being pulled in 
    public int fishNum;
    
    // When reaching a threshold, player will need to input the correct spin movement in time.
    public float scratchTimer;

    // Determines when the countdown starts for the scratch timer.
    private float scratchTimerStart;
    [SerializeField]
    private float minigameTimer;
    private bool hasScratchPrompted;
    public bool isFishMode;
    private Vector2 originalRingSize;

    private PlayerController controller;
    private PlayerInputController inputController;

    public GameObject[] fishLogEntries;
    void Awake()
    {
        // finds the respective scripts within this script's GameObject.
        controller = GetComponent<PlayerController>();
        inputController = GetComponent<PlayerInputController>();

        // Sets all variables to needed values.
        scratchTimer = Random.Range(5, 10);
        hasScratchPrompted = false;
        timerRing.SetActive(false);
        originalRingSize = timerRing.GetComponent<RectTransform>().sizeDelta;
        isFishMode = false;
    }

    void Update()
    {
        if (isFishMode)
        {
            scratchTimer -= Time.deltaTime;
        }
        
        // Start the record scratch prompt.
        if (scratchTimer <= 3 && !hasScratchPrompted)
        {
            timerRing.SetActive(true);
            hasScratchPrompted = true;
        }
        else if (scratchTimer <= scratchTimerStart)
        {
            float ringWidth = timerRing.GetComponent<RectTransform>().sizeDelta.x;
            float ringHeight = timerRing.GetComponent<RectTransform>().sizeDelta.y;
            timerRing.GetComponent<RectTransform>().sizeDelta = new Vector2(ringWidth - (ringHeight * Time.deltaTime), ringHeight - (ringHeight * Time.deltaTime));
            
            // Check for if the player correctly scratches the disk
            if (inputController.movementInputVector.z < 0)
            {
                ResetTimer();
            }
        }

        if (scratchTimer <= 0)
        {
            Fail();
        }
    }

    public void ResetTimer()
    {
        scratchTimer = Random.Range(5, 10);
        hasScratchPrompted = false;
        timerRing.SetActive(false);
        timerRing.GetComponent<RectTransform>().sizeDelta = originalRingSize;
    }
    public void StartFishing()
    {
        fishingRod.SetActive(true);
        fishingUI.SetActive(true);
        isFishMode = true;
        
        fishNum = Random.Range(0, fishLogEntries.Length);
            
        // Easy
        if (fishNum == 0)
        {
            scratchTimerStart = 3;
        }
        // Medium
        else if (fishNum == 1)
        {
            scratchTimerStart = 2.5f;
        }
        // Hard
        else if (fishNum == 2)
        {
            scratchTimerStart = 2;
        }
    }
    public void StopFishing()
    {
        ResetTimer();
        fishingUI.SetActive(false);
        isFishMode = false;
        inputController.isMoveMode = true;
        fishingRod.SetActive(false);
    }
    public void Fail()
    {
        //
        // Display a fail popup
        //
        fishLogEntries[fishNum].SetActive(true);
        StopFishing();
        //Invoke("StopFishing", 3);
    }
    public void Success()
    {
        //
        // Display a Success popup
        //
        fishLogEntries[fishNum].SetActive(true);
        Invoke("StopFishing", 3);
    }

    // Check method to allow other scripts if fishing is in fish mode.
    public bool CheckIfFishing()
    {
        if (isFishMode) return true;
        else return false;
    }
}
