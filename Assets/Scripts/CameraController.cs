using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform objectToFollow;

    void Start()
    {
        objectToFollow = GameObject.Find("Character").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(objectToFollow.position.x, objectToFollow.position.y, transform.position.z);
    }
}
