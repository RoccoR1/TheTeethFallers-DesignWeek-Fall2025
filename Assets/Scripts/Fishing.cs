using UnityEngine;

public class Fishing : MonoBehaviour
{
    public GameObject timerRing;
    public GameObject fishingUI;
    public GameObject[] fishLogEntries;
    public int fishNum;
    public float scratchTimer;
    private bool hasScratchPrompted;
    public bool isFishMode;
    private Vector2 originalRingSize;

    private PlayerController controller;
    private PlayerInputController inputController;
    
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
        if (scratchTimer <= 3 && !hasScratchPrompted)
        {
            timerRing.SetActive(true);
            hasScratchPrompted = true;
        }
        else if (scratchTimer <= 3)
        {
            float ringWidth = timerRing.GetComponent<RectTransform>().sizeDelta.x;
            float ringHeight = timerRing.GetComponent<RectTransform>().sizeDelta.y;
            timerRing.GetComponent<RectTransform>().sizeDelta = new Vector2(ringWidth - (ringHeight * Time.deltaTime), ringHeight - (ringHeight * Time.deltaTime));
        }
        if (scratchTimer <= 0)
        {
            ResetTimer();
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
        fishingUI.SetActive(true);
        isFishMode = true;
        fishNum = Random.Range(0, 2);
    }
    public void StopFishing()
    {
        ResetTimer();
        fishingUI.SetActive(false);
        isFishMode = false;
    }
}
