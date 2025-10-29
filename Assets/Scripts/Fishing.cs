using UnityEngine;

public class Fishing : MonoBehaviour
{
    public GameObject timerRing;
    public GameObject fishingUI;
    public float scratchTimer;
    public bool hasScratchPrompted;
    public bool isFishMode;
    private Vector2 originalRingSize;

    private PlayerController controller;
    private PlayerInputController inputController;
    
    void Awake()
    {
        controller = GetComponent<PlayerController>();
        inputController = GetComponent<PlayerInputController>();
        scratchTimer = Random.Range(5, 10);
        hasScratchPrompted = false;
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
            timerRing.GetComponent<RectTransform>().sizeDelta = new Vector2(timerRing.GetComponent<RectTransform>().sizeDelta.x - Time.deltaTime, timerRing.GetComponent<RectTransform>().sizeDelta.y - Time.deltaTime);
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
    }
    public void StopFishing()
    {
        ResetTimer();
        fishingUI.SetActive(false);
        isFishMode = false;
    }
}
