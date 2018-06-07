using System;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenBehaviour : MonoBehaviour {
    [SerializeField] float maxTime;

    float timeLeft = 10f;

    private void Awake() {
        timeLeft = maxTime;
    }

    public void Update() {
        timeLeft -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Return) || timeLeft < 0f)
            EndEndScreen();
    }

    private void EndEndScreen() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}