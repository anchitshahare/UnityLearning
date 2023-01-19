using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;
    private int _bestScore;
    // Start is called before the first frame update
    private void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _bestText.text = "Best: " + _bestScore;

        if(_gameManager == null) {
            Debug.LogError("GameManager is null");
        }
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int playerScore) {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void BestScore(int currentScore) {
        if(currentScore > _bestScore) {
            _bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", _bestScore);
            _bestText.text = "Best: " + _bestScore;
            
        }
    }

    public void UpdateLives(int currentLives) {
        _livesImage.sprite = _livesSprite[currentLives];
        
        if(currentLives == 0) {
            GameOverSequence();
        }
        
    }

    void GameOverSequence() {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
             
        if(Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log(SceneManager.GetActiveScene().name);
        }
        StartCoroutine(GameOverFlicker()); 
    }

    IEnumerator GameOverFlicker() {
        while(true) {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    public void ResumePlay() {
        GameManager gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        gameManager.ResumeGame();
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("Main_Menu"); 
    }
}
