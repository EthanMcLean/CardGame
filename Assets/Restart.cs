using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public AudioSource laughing;
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !laughing.isPlaying)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
