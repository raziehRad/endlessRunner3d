using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreTxt;
    [SerializeField] private TextMeshProUGUI _healthTxt;
   
    private int score;
    private int health;
    private int fullHealth=100;

    private void Awake()
    {
        health = fullHealth;
        var highScore=PlayerPrefs.GetInt("HighScore");
        _scoreTxt.text = highScore.ToString();
    }

    public void SetScore(int value)
    {
        score += value;
        _scoreTxt.text = score.ToString();
    }

    public void SetHealth(int value)
    {
        health -= value;
        if (health <= 0) SaveData();
        _healthTxt.text = health.ToString();
    }

    public void SaveData()
    {
        AudioManager.instance.PlayGameOverSound();
        var highScore=PlayerPrefs.GetInt("HighScore");
        if (score>highScore) PlayerPrefs.SetInt("HighScore",score);
        Invoke("LoadGame",0.5f);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(0);
    }
}