using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Changing world");
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("touching ground");
        transform.parent.SendMessage("Testing");
    }
}
