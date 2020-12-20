using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject[] slots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].transform.childCount == 0)
                {
                    gameObject.layer = 5;
                    gameObject.transform.position = slots[i].transform.position;
                    gameObject.transform.SetParent(slots[i].transform);

                    RectTransform ingredientTransform = gameObject.GetComponent<RectTransform>();
                    ingredientTransform.offsetMin = new Vector2(0, 0);
                    ingredientTransform.offsetMax = new Vector2(0, 0);
                    ingredientTransform.anchorMin = new Vector2(0, 0);
                    ingredientTransform.anchorMax = new Vector2(1, 1);
                    ingredientTransform.pivot = new Vector2(0.5f, 0.5f);
                    ingredientTransform.localScale = new Vector3(1, 1, 0);
                    break;
                }
            }
        }
    }
}
