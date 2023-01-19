using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    [SerializeField]
    private Animator _animator;

    private void Start() {
        _animator = GameObject.Find("PauseMenuPanel").GetComponent<Animator>();
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true) {
            SceneManager.LoadScene(1); // main menu
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.P)) {
            _animator.SetBool("isPaused", true);
            Time.timeScale = 0;
            _pauseMenuPanel.SetActive(true);
        }
    }

    public void GameOver() {
        _isGameOver = true;
    }

    public void ResumeGame() {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
