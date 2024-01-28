using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject gameplay;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            gameObject.SetActive(false);
            gameplay.SetActive(true);
            GameManager.instance.StartGame();
        }
    }
}
