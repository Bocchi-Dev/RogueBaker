using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("UI Elements")]
    public GameObject[] inventorySlots;
    public TextMeshProUGUI timerText;
    public GameObject timerPanel;
    public GameObject taskbar;
    public GameObject collectRupees;
    public TextMeshProUGUI rupeesAmountText;
    public float timerValue;
    private float timer;

    [Header("Utilities")]
    public bool gameBegins = false;
    public bool GameOver = false;
    public bool inventoryFull = false;
    public bool startTimer = false;
    public bool pauseTimer = false;
    public bool rebellion = false;
 
    [Header("Dialogue Stuff")]
    public bool ConversationActive = false;
    public bool firstMeeting = true;
    public bool firstMeetingDialogueDone = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = timerValue;
        timerPanel.SetActive(false);
        taskbar.SetActive(false);

        goToLevel("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {
        checkFullInventory();

        //pause timer during conversations
        if (ConversationActive)
        {
            pauseTimer = true;
        }
        else
        {
            pauseTimer = false;
        }

        //start timer after being commanded by king
        if (gameBegins)
        {
            startTimer = true;
            taskbar.SetActive(true);
        }

        if (startTimer)
        {
            timerPanel.SetActive(true);
            if (!pauseTimer)
            {
                Timer();
            }
        }

        //condition if timer reaches 0
        if(timer < 0)
        {
            outOfTime();
        }
    }

    //check if inventory is full
    public void checkFullInventory()
    {
        if (inventorySlots[0].transform.childCount > 0 && inventorySlots[1].transform.childCount > 0 
            && inventorySlots[2].transform.childCount > 0 && inventorySlots[3].transform.childCount > 0)
        {
            inventoryFull = true;
        }
        else
        {
            inventoryFull = false;
        }
    }

    public void emptyInventory()
    {
        foreach(GameObject item in inventorySlots)
        {
            Destroy(item.transform.GetChild(0).gameObject);             
        }
    }

    //handle timer
    public void Timer()
    {
        timer -= Time.deltaTime;

        int minute = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = minute.ToString("00") + ":" + seconds.ToString("00");
    }

    //handle if time runs out
    public void outOfTime()
    {
        //condition for if timer is 0
        //explode and go to game over screen
    }
    public void playerHurt()
    {

    }

    public void goToLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
