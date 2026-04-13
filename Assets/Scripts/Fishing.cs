using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [Header("Models & UI")]
    public GameObject fishingRod;
    public GameObject fish;
    public MeshRenderer fishBody;
    public GameObject fishingUI;
    public GameObject waitingText;
    public GameObject fishCDSprite;
    public GameObject rotateRightSymbol;
    public GameObject rotateLeftSymbol;
    public GameObject rotateSymbolBkg;
    public GameObject turnText;
    public GameObject timerText;
    public GameObject winText;
    public GameObject losetext;

    [Header("FMOD Settings")]
    public string musicEventPath = "event:/Music";
    [Range(0f, 1f)]
    public float musicVolume = 0.5f; // Set this in the Inspector! 1.0 is full, 0.0 is silent.
    private FMOD.Studio.EventInstance musicInstance;

    [Header("Minigame Settings")]
    public int fishNum;
    public float scratchTimer;
    private float scratchStartPromptTime;
    public float minigameTimer;
    public bool isFishMode;
    private bool hasScratchPrompted;

    [SerializeField] private float spinTimeSample;
    private float wheelRotationDuringSample;
    private float wheelMinSpinPer;
    private float wheelMaxSpinPer;

    [Header("Player References")]
    private PlayerController controller;
    private PlayerInputController inputController;
    public GameObject[] fishLogEntries;
    public TextMeshProUGUI[] fishLogCount;

    private float reelingSfxTimer;
    private float reelingSfxInterval = 0.2f;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        inputController = GetComponent<PlayerInputController>();
        isFishMode = false;
        fish.SetActive(false);
        fishingRod.SetActive(false);
        fishingUI.SetActive(false);
    }

    void Start()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEventPath);

        // --- VOLUME CONTROL ---
        musicInstance.setVolume(musicVolume);

        musicInstance.start();
        musicInstance.setParameterByName("IsFishing", 0f);
    }

    void Update()
    {
        // Keep the volume synced with the Inspector value in real-time
        musicInstance.setVolume(musicVolume);

        if (isFishMode)
        {
            scratchTimer -= Time.deltaTime;
            minigameTimer -= Time.deltaTime;
            timerText.GetComponent<TextMeshProUGUI>().text = minigameTimer.ToString("F2");
            CheckRotation();

            if (scratchTimer <= 3 && !hasScratchPrompted)
            {
                fishCDSprite.SetActive(true);
                hasScratchPrompted = true;
                spinTimeSample = scratchTimer;
                wheelRotationDuringSample = 0;
                rotateLeftSymbol.SetActive(true);
                rotateRightSymbol.SetActive(false);
            }
            else if (scratchTimer <= scratchStartPromptTime)
            {
                fishCDSprite.GetComponent<RectTransform>().Rotate(0, 0, 1);
            }
            else
            {
                fishCDSprite.GetComponent<RectTransform>().Rotate(0, 0, -1);
            }

            if (scratchTimer <= 0) Fail();
            if (minigameTimer <= 0) Success();
        }
    }

    private void CheckRotation()
    {
        spinTimeSample -= Time.deltaTime;
        float rotationInput = inputController.movementInputVector.z;
        wheelRotationDuringSample += rotationInput;

        if (Mathf.Abs(rotationInput) > 0.1f && Time.time > reelingSfxTimer)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Reel SFX");
            reelingSfxTimer = Time.time + reelingSfxInterval;
        }

        if (spinTimeSample <= 0)
        {
            if (!hasScratchPrompted)
            {
                if (wheelRotationDuringSample <= wheelMinSpinPer) Fail();
            }
            else
            {
                if (wheelRotationDuringSample > 0) Fail();
                else ResetScratchPrompt();
            }
        }
    }

    public void ResetScratchPrompt()
    {
        scratchTimer = Random.Range(5, 10);
        hasScratchPrompted = false;
        rotateLeftSymbol.SetActive(false);
        rotateRightSymbol.SetActive(true);
        spinTimeSample = 4;
        wheelRotationDuringSample = 0;
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
        rotateRightSymbol.SetActive(false);
        rotateSymbolBkg.SetActive(false);
        fishCDSprite.SetActive(false);
        turnText.SetActive(false);
        timerText.SetActive(false);
        fishingUI.SetActive(false);

        isFishMode = false;
        inputController.isMoveMode = true;
        fishingRod.SetActive(false);
        fish.SetActive(false);

        musicInstance.setParameterByName("IsFishing", 0f);
    }

    public void Fail()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/fish fail");
        rotateRightSymbol.SetActive(false);
        rotateLeftSymbol.SetActive(false);
        fishCDSprite.SetActive(false);
        losetext.SetActive(true);
        isFishMode = false;
        Invoke("StopFishing", 3);
    }

    public void Success()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/fish dialogue catch");
        rotateRightSymbol.SetActive(false);
        rotateLeftSymbol.SetActive(false);
        fishCDSprite.SetActive(false);
        winText.SetActive(true);
        isFishMode = false;
        fishLogEntries[fishNum].SetActive(true);
        fishLogCount[fishNum].text = (int.Parse(fishLogCount[fishNum].text) + 1).ToString();
        Invoke("StopFishing", 3);
    }

    private void warnPlayer() => waitingText.GetComponent<TextMeshProUGUI>().text = "!";

    private void SetupFishing()
    {
        waitingText.GetComponent<TextMeshProUGUI>().text = ". . .";
        waitingText.SetActive(false);
        rotateRightSymbol.SetActive(true);
        rotateSymbolBkg.SetActive(true);
        fishCDSprite.SetActive(true);
        turnText.SetActive(true);
        timerText.SetActive(true);

        isFishMode = true;
        fishNum = Random.Range(0, fishLogEntries.Length);

        musicInstance.setParameterByName("IsFishing", 1f);

        // Difficulty Settings
        if (fishNum == 0) { scratchStartPromptTime = 3; minigameTimer = 12; wheelMinSpinPer = 1; fishBody.material.color = Color.yellow; }
        else if (fishNum == 1) { scratchStartPromptTime = 2.75f; minigameTimer = 15; wheelMinSpinPer = 500; fishBody.material.color = new Color(0.85f, 0.74f, 0.42f); }
        else if (fishNum == 2) { scratchStartPromptTime = 2.5f; minigameTimer = 16; wheelMinSpinPer = 1000; fishBody.material.color = Color.pink; }
        else if (fishNum == 3) { scratchStartPromptTime = 2.25f; minigameTimer = 18; wheelMinSpinPer = 1000; fishBody.material.color = new Color(0.8f, 0.36f, 0.36f); }
        else if (fishNum == 4) { scratchStartPromptTime = 2f; minigameTimer = 20; wheelMinSpinPer = 1000; fishBody.material.color = new Color(0.5f, 0f, 0.5f); }
        else if (fishNum == 5) { scratchStartPromptTime = 1.5f; minigameTimer = 20; wheelMinSpinPer = 1000; fishBody.material.color = Color.green; }

        FMODUnity.RuntimeManager.PlayOneShot("event:/fish dialogue 1");
        fish.transform.position = this.transform.position + (this.transform.forward * 6) + (Vector3.down * 8);
        fish.transform.rotation = this.transform.rotation;
        fish.SetActive(true);
    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }

    public bool CheckIfFishing() => isFishMode;
}