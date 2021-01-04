using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameController.instance.GameOver = false;
            GameController.instance.gameBegins = false;
            GameController.instance.inventoryFull = false;
            GameController.instance.startTimer = false;
            GameController.instance.pauseTimer = false;
            GameController.instance.rebellion = false;
            GameController.instance.playerDead = false;
            GameController.instance.bossFightTime = false;
            GameController.instance.ConversationActive = false;
            GameController.instance.firstMeetingKing = true;
            GameController.instance.firstMeetingFairy = true;
            GameController.instance.firstMeetingDialogueDone = false;
            GameController.instance.kingHealth = 4;
            GameController.instance.emptyInventory();
            GameController.instance.resetTimer();
            GameController.instance.restoreMaxHealth();

            SceneManager.LoadScene("MainMenu");
        }
    }
}
