using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _scoreText;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Image _livesImage;
    [SerializeField] private TMPro.TMP_Text _gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "SCORE: " + 0;
        _gameOverText.gameObject.SetActive(false);
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "SCORE: " + playerScore.ToString();
    }  

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
    }
}
