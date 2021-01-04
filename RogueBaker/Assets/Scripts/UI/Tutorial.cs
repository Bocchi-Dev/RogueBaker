using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    public UnityEvent triggerInstruction;
    public UnityEvent offInstruction;

    private float timer = 3f;

    private void Update()
    {
        
    }

    IEnumerator countdown()
    {
        yield return new WaitForSeconds(timer);

        offInstruction.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!GameController.instance.tutorialDone)
            {
                triggerInstruction.Invoke();
            }          
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(countdown());
    }


}
