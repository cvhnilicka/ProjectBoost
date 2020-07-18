using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // variables n stuff
    Rigidbody rigidbody;
    AudioSource audioSource;

    [SerializeField]
    float rcsThrust = 150f;

    [SerializeField]
    float boosterThrust = 1000f;

    [SerializeField] float levelLoadDelay = 1f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    private int numLevels;
    private int currLevel = 0;
    bool collisionsOn = true;

    enum State {  Alive, Dying, Transcending }

    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        numLevels = SceneManager.sceneCountInBuildSettings;
        // print(numLevels);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }
        if (Debug.isDebugBuild)
            RespondToDebugKeys();

    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            // toggle collisions
            collisionsOn = !collisionsOn;

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !collisionsOn) return;


        switch (collision.gameObject.tag)
        {
            case "Friendly": // do nothing
                break;
                
            case "Finish": Success();
                break;
            default: Die();
                break;
        }
    }

    private void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if ( nextIndex >= numLevels)
        {
            nextIndex = 0;
        }
        SceneManager.LoadScene(nextIndex);

    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Success()
    {
        print("currLevel: " + currLevel);
        print("numLevels: " + numLevels);
        state = State.Transcending;
        audioSource.Stop();
        successParticles.Play();
        audioSource.PlayOneShot(successSound);
        Invoke("LoadNextLevel", levelLoadDelay); // parameterize next time
    }

    private void Die()
    {
        state = State.Dying;
        audioSource.Stop();
        engineParticles.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadFirstLevel", levelLoadDelay);
    }




    // Process input
    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            // space key is down
            ApplyThrust();
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
            // stop when no longer thrusting
            audioSource.Stop();
            engineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * boosterThrust * Time.deltaTime);

        // only start playing if it isnt already
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);

        if (!engineParticles.isPlaying)
            engineParticles.Play();
    }
}
