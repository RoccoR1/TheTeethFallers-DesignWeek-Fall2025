using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public GameObject fishingRod;

    // UI Objects
    public GameObject fishingUI;
    public GameObject waitingText;
    public GameObject fishCDSprite;
    public GameObject rotateRightSymbol;
    public GameObject rotateLeftSymbol;
    public GameObject rotateSymbolBkg;
    public GameObject turnText;

    // Music
    public GameObject boatMusic;
    public GameObject minigameMusic;

    // Determines which fish is currently being pulled in 
    public int fishNum;
    
    // When reaching a threshold, player will need to input the correct spin movement in time.
    public float scratchTimer;

    // Timers
    // Determines when the countdown starts for the scratch timer.
    private float scratchStartPromptTime;
    [SerializeField]
    private float minigameTimer;

    // Booleans
    private bool hasScratchPrompted;
    public bool isFishMode;

    [SerializeField]
    private float spinTimeSample;
    private float wheelRotationDuringSample;
    private float wheelMinSpinPer;
    private float wheelMaxSpinPer;

    public GameObject winText;
    public GameObject losetext;
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
        isFishMode = false;
    }

    void Update()
    {
        if (isFishMode)
        {
            scratchTimer -= Time.deltaTime;
            minigameTimer -= Time.deltaTime;

            CheckRotation();

            // Start the record scratch prompt.
            if (scratchTimer <= 3 && !hasScratchPrompted)
            {
                fishCDSprite.SetActive(true);
                hasScratchPrompted = true;
                spinTimeSample = scratchTimer;
                wheelRotationDuringSample = 0;
                rotateLeftSymbol.SetActive(true);
                rotateRightSymbol.SetActive(false);
            }

            //Runs constantly after the timer passes the prompt threshold.
            else if (scratchTimer <= scratchStartPromptTime)
            {
                fishCDSprite.GetComponent<RectTransform>().Rotate(0,0,1);
            }
            else
            {
                fishCDSprite.GetComponent<RectTransform>().Rotate(0, 0, -1);
            }

            if (scratchTimer <= 0)
            {
                Fail();
            }
            if (minigameTimer <= 0)
            {
                Success();
            }
        }
    }
    // This method will check if the player is spinning their turntable enough in a given period of time. If they do not, then they fail. Otherwise fine.
    private void CheckRotation()
    {
        spinTimeSample -= Time.deltaTime;
        wheelRotationDuringSample += inputController.movementInputVector.z;
        //Debug.Log(wheelRotationDuringSample);
        if (spinTimeSample <= 0)
        {
            if (!hasScratchPrompted)
            {
                // Statement checks to see if the rotation value is within the required range.
                if (wheelRotationDuringSample <= wheelMinSpinPer || wheelRotationDuringSample >= wheelMaxSpinPer)
                {
                    Fail();
                }
            }
            else
            {
                if (wheelRotationDuringSample > 0)
                {
                    Fail();
                }
                else
                {
                    ResetScratchPrompt();
                }
            }
            spinTimeSample = 4;
            wheelRotationDuringSample = 0;
        }
    }

    public void ResetScratchPrompt()
    {
        scratchTimer = Random.Range(5, 10);
        hasScratchPrompted = false;
        rotateLeftSymbol.SetActive(false);
        rotateRightSymbol.SetActive(true);
    }
    public void StartFishing()
    {
        fishingRod.SetActive(true);
        fishingUI.SetActive(true);
        waitingText.SetActive(true);
        float waitTime = Random.Range(2, 3);
        Invoke("warnPlayer", waitTime);
        Invoke("SetupFishing", waitTime + 1);
    }
    public void StopFishing()
    {
        ResetScratchPrompt();
        winText.SetActive(false);
        losetext.SetActive(false);
        fishingUI.SetActive(false);
        isFishMode = false;
        inputController.isMoveMode = true;
        fishingRod.SetActive(false); 
        boatMusic.SetActive(!boatMusic.activeSelf);
        minigameMusic.SetActive(!minigameMusic.activeSelf);
    }
    public void Fail()
    {
        //
        // Display a fail popup
        //
        Debug.Log("Fail!");
        rotateRightSymbol.SetActive(false);
        rotateLeftSymbol.SetActive(false);
        fishCDSprite.SetActive(false);
        losetext.SetActive(true);

        isFishMode = false;
        Invoke("StopFishing", 3);
    }
    public void Success()
    {
        //
        // Display a Success popup
        //
        Debug.Log("Success!");

        rotateRightSymbol.SetActive(false);
        rotateLeftSymbol.SetActive(false);
        fishCDSprite.SetActive(false);
        winText.SetActive(true);

        isFishMode = false;
        fishLogEntries[fishNum].SetActive(true);
        Invoke("StopFishing", 3);
    }

    // Informs player that the fishing minigame is about to begin.
    private void warnPlayer()
    {
        waitingText.GetComponent<TextMeshProUGUI>().text = "!";
    }
    private void SetupFishing()
    {
        // Deactivate waiting text
        waitingText.SetActive(false);
        
        // Actvate all required UI.
        rotateRightSymbol.SetActive(true);
        rotateSymbolBkg.SetActive(true);
        fishCDSprite.SetActive(true);
        turnText.SetActive(true);

        isFishMode = true;

        fishNum = Random.Range(0, fishLogEntries.Length);

        // Easy
        if (fishNum == 0)
        {
            scratchStartPromptTime = 3;
            minigameTimer = 15;
            wheelMinSpinPer = 1;
            wheelMaxSpinPer = 20000;
        }
        // Medium
        else if (fishNum == 1)
        {
            scratchStartPromptTime = 2.5f;
            minigameTimer = 15;
            wheelMinSpinPer = 1;
            wheelMaxSpinPer = 20000;
        }
        // Hard
        else if (fishNum == 2)
        {
            scratchStartPromptTime = 2;
            minigameTimer = 20;
            wheelMinSpinPer = 1;
            wheelMaxSpinPer = 20000;
        }
        boatMusic.SetActive(!boatMusic.activeSelf);
        minigameMusic.SetActive(!minigameMusic.activeSelf);
    }

    // Check method to allow other scripts if fishing is in fish mode.
    public bool CheckIfFishing()
    {
        if (isFishMode) return true;
        else return false;
    }
}
