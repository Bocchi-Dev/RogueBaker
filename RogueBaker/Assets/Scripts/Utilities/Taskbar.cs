using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taskbar : MonoBehaviour
{
    private Animator anime;
    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleTasks()
    {
        Debug.Log("toggled");
        if (anime.GetBool("openTasks"))
        {
            anime.SetBool("openTasks", false);
        }
        else
        {
            anime.SetBool("openTasks", true);
        }
        
    }
}
