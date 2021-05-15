using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D rigidbody;
    private float jumpSpeed = 20.0f;
    private bool canJump = false;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Changing worlds");
        }
        if (Input.GetKeyDown(KeyCode.Space) && canJump) {
            rigidbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            canJump = false;
        } 
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            rigidbody.AddForce(new Vector2(-2, 0) * jumpSpeed);
        } 
        else if(Input.GetKeyDown(KeyCode.E)) {
            //Debug.Log("Changing world");
        }
    }

    public void Testing()
    {
        canJump = true;
    }

}
