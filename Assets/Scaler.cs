using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{

    [SerializeField] Vector3 scalingVector = new Vector3(5f, 5f, 5f);

    float scalingFactor;

    [SerializeField] float period = 4f;


    private Vector3 startingScale;

    // Start is called before the first frame update
    void Start()
    {
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) return;
        float cycles = Time.time / period; // grows continually from 0
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        scalingFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = scalingVector * scalingFactor;
        transform.localScale = offset + startingScale;
    }
}
