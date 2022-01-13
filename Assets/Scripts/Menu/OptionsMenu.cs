using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider _gameSessionTimeSlider;
    [SerializeField] private Slider _enemySpawnTimeSlider;
    [SerializeField] private TextMeshProUGUI _gameSessionTimeFeedbackText;
    [SerializeField] private TextMeshProUGUI _enemySpawnTimeFeedbackText;

    void Start()
    {
        float sessionTime = PlayerPrefs.GetFloat("GameSessionTime");
        float enemySpawnTime = PlayerPrefs.GetFloat("EnemySpawnTime");
        if(sessionTime == 0 || enemySpawnTime == 0) 
        {
            PlayerPrefs.SetFloat("GameSessionTime", _gameSessionTimeSlider.value);
            PlayerPrefs.SetFloat("EnemySpawnTime", _enemySpawnTimeSlider.value);
        }
        else
        {
            _gameSessionTimeSlider.value = sessionTime;
            _enemySpawnTimeSlider.value = enemySpawnTime;
        }        
    }

    public void SetGameSessionTime(float time)
    {
        PlayerPrefs.SetFloat("GameSessionTime", time);
    }

    public void SetEnemySpawnTime(float time)
    {
        PlayerPrefs.SetFloat("EnemySpawnTime", time);
    }

    public void DisplayGameSessionTime()
    {
        _gameSessionTimeFeedbackText.SetText(_gameSessionTimeSlider.value.ToString("F1"));
    }

    public void DisplayEnemySpawnTime()
    {
        _enemySpawnTimeFeedbackText.SetText(_enemySpawnTimeSlider.value.ToString("F1"));
    }
}
