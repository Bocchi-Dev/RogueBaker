using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("UI Elements")]
    public GameObject[] inventorySlots;
    public TextMeshProUGUI timerText;
    public GameObject timerPanel;
    public GameObject taskbar;
    public GameObject collectRupees;
    public int rupeeAmount = 0;
    public TextMeshProUGUI rupeesAmountText;
    public float timerValue;
    private float timer;
    public Slider playerHealthBar;

    [Header("Utilities")]
    public bool gameBegins = false;
    public bool GameOver = false;
    public bool inventoryFull = false;
    public bool startTimer = false;
    public bool pauseTimer = false;
    public bool rebellion = false;
    public bool playerDead = false;
    public bool bossFightTime = true;
    public bool tutorialDone = false;
 
    [Header("Dialogue Stuff")]
    public bool ConversationActive = false;
    public bool firstMeetingKing = true;
    public bool firstMeetingFairy = true;
    public bool firstMeetingDialogueDone = false;

    [Header("King Variables")]
    [Range(4, 15)]
    public float kingHealth;

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

        if (!rebellion)
        {
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
        }

        //condition if timer reaches 0
        if(timer < 0)
        {
            outOfTime();
        }

        if(playerHealthBar.value <= 0)
        {
            playerDead = true;
        }

        if (!bossFightTime)
        {
            if (playerDead)
            {
                playerDied();
            }
        }
        else if(bossFightTime && playerDead)
        {
            endGame();
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
            try
            {
                Destroy(item.transform.GetChild(0).gameObject);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
                       
        }

        inventoryFull = false;
    }

    //handle timer
    public void Timer()
    {
        timer -= Time.deltaTime;

        int minute = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = minute.ToString("00") + ":" + seconds.ToString("00");
    }

    public void resetTimer()
    {
        timer = timerValue;
    }

    //handle if time runs out
    public void outOfTime()
    {
        endGame();
    }

    public void endGame()
    {
        FindObjectOfType<SceneTransitions>().endGame();
    }

    //player stuff
    public void playerHurt()
    {
        playerHealthBar.value -= 1;
    }

    public void playerDied()
    {
        FindObjectOfType<SceneTransitions>().playerDied();
        restoreMaxHealth();
    }

    public void restoreMaxHealth()
    {
        playerHealthBar.value = playerHealthBar.maxValue;
    }

    public void addRupee()
    {
        rupeeAmount++;
        rupeesAmountText.text = rupeeAmount.ToString();
    }

    //king stuff
    public void strengthenKing()
    {
        kingHealth += 2;
    }

    public void removeTimer()
    {
        pauseTimer = true;
        rebellion = true;
        startTimer = false;
        timerPanel.SetActive(false);
    }

    public void goToLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void restartGame()
    {
        GameOver = false;
        gameBegins = false;
        inventoryFull = false;
        startTimer = false;
        pauseTimer = false;
        rebellion = false;
        playerDead = false;
        bossFightTime = false;
        ConversationActive = false;
        firstMeetingKing = true;
        firstMeetingFairy = true;
        firstMeetingDialogueDone = false;
        kingHealth = 4;
        emptyInventory();
        timer = timerValue;
        timerPanel.SetActive(false);
        taskbar.SetActive(false);
        restoreMaxHealth();
        timerPanel.SetActive(false);
    }
}
