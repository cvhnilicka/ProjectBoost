using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // variables n stuff
    Rigidbody rigidbody;
    AudioSource audioSource;

    [SerializeField]
    float rcsThrust = 150f;

    [SerializeField]
    float boosterThrust = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Thrust();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly": // do nothing
                break;
            default: print("EXPLOSION!!!!");
                break;
        }
    }


    // Process input
    private void Rotate()
    {

        // need to prevent unwanted spinning
        rigidbody.freezeRotation = true; // take manuel control of rotation (from physics system)


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

        rigidbody.freezeRotation = false; // resume physics control of body
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            // space key is down
            rigidbody.AddRelativeForce(Vector3.up * boosterThrust * Time.deltaTime);

            // only start playing if it isnt already
            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
            // stop when no longer thrusting
            audioSource.Stop();
        }
    }
}
