using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject[] inventorySlots;

    public bool ConversationActive = false;
    public bool GameOver = false;
    public bool inventoryFull = false;

    
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
        
    }

    // Update is called once per frame
    void Update()
    {
        checkFullInventory();
    }

    public void checkFullInventory()
    {
        foreach(GameObject slot in inventorySlots)
        {
            if(slot.transform.childCount > 0)
            {
                inventoryFull = true;
            }
            else
            {
                inventoryFull = false;
            }
        }
        Debug.Log(inventoryFull);
    }

    public void playerHurt()
    {

    }
}
