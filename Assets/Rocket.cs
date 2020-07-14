using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // variables n stuff
    Rigidbody rigidbody;
    const float rcsThrust = 100f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }
    // Process input
    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            // space key is down
            print("Space pressed");
            rigidbody.AddRelativeForce(Vector3.up * 10f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            // rotate left 
            print("rotate left");
            transform.Rotate(Vector3.forward * rcsThrust * Time.deltaTime);
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            print("rotate right");
            transform.Rotate(Vector3.forward * -rcsThrust * Time.deltaTime);
        }
    }
}
