using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // variables n stuff
    Rigidbody rigidbody;
    AudioSource audioSource;
    const float rcsThrust = 100f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
            rigidbody.AddRelativeForce(Vector3.up * 10f);

            // only start playing if it isnt already
            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
            // stop when no longer thrusting
            audioSource.Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            // rotate left 
            transform.Rotate(Vector3.forward * rcsThrust * Time.deltaTime);
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            // rotate right
            transform.Rotate(Vector3.forward * -rcsThrust * Time.deltaTime);
        }
    }
}
