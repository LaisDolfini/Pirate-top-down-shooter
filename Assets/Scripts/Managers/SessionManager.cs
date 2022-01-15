using System;
using UnityEngine;
using TMPro;

public class SessionManager : MonoBehaviour
{
    [Header("Required Components---------")]
    [SerializeField] private Life _playerLife;
    [SerializeField] private GameObject _finishedSessionGroup;

    [Header("Required Texts---------")]
    [SerializeField] private TextMeshProUGUI _finishedSessionText, _inGameScoreText, _finalScoreText, _timerText;

    [Header("Required Sounds---------")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] AudioClip _victory, _gameOver;

    private float _sessionTime, _sessionTimer;
    private float _sessionTimeMinutes, _sessionTimeSeconds;
    private bool _startSessionTimer;

    [HideInInspector] public float _score;
    
    void Awake()
    {
        _playerLife.onDied += GameOver;
    }

    void OnDisable()
    {
        _playerLife.onDied -= GameOver;
    }

    void Start()
    {
        _sessionTime = PlayerPrefs.GetFloat("GameSessionTime");
        _sessionTimer = _sessionTime * 60;

        _startSessionTimer = true;
    }

    void Update()
    {
        if(!_startSessionTimer) return;

        _sessionTimer -= Time.deltaTime;
        SetTimerText();

        if(_sessionTimer <= 0) WonGameSession();
    }

    void SetTimerText()
    {
        int minutes = TimeSpan.FromSeconds(_sessionTimer).Minutes;
        float seconds = TimeSpan.FromSeconds(_sessionTimer).Seconds;
        TimeDisplay(minutes, seconds, _timerText);
    }

    void TimeDisplay(int minutes, float seconds, TextMeshProUGUI text)
    {   
        if(seconds < 10)
        {
            if(minutes < 10) text.SetText("0" + minutes + ":0" + (int)seconds);
            else text.SetText(minutes + ":0" + (int)seconds);
        }
        else
        {
            if(minutes < 10) text.SetText("0" + minutes + ":" + (int)seconds);
            else text.SetText(minutes + ":" + (int)seconds);
        }
    }

    public void SetScore()
    {
        _score++;
        _inGameScoreText.SetText("Score: " + _score.ToString());
    }

    void WonGameSession()
    {
        Time.timeScale = 0;
        _finishedSessionText.SetText("You Won!");
        _finalScoreText.SetText("Score: " + _score.ToString());
        _finishedSessionGroup.SetActive(true);
        _startSessionTimer = false;
        _audioSource.PlayOneShot(_victory);
    }

    public void GameOver()
    {
        _startSessionTimer = false;
        Time.timeScale = 0;
        _finishedSessionText.SetText("Game Over");
        _finalScoreText.SetText("Score: " + _score.ToString());
        _finishedSessionGroup.SetActive(true);
        _audioSource.PlayOneShot(_gameOver);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
