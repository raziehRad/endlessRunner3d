using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] GameObject _continue;
    [SerializeField] private TextMeshProUGUI _scoreTxt;
    [SerializeField] private TextMeshProUGUI _healthTxt;
    [SerializeField] private TextMeshProUGUI _boostedtxt;
    [SerializeField] private GameObject _boostedIcon;
    [SerializeField] private GameObject _boostedItem;
    [SerializeField] private int _boostedTimer;
    [SerializeField] private GroundSpawner _spawner;
    private int boostedCount;
    
    private int score;
    private int health;
    private int fullHealth=100;
   [SerializeField] private int boostedSpeed=10;

    private void Awake()
    {
        instance = this;
        Initialize();
    }

    private void Initialize()
    {
        health = fullHealth;
        var highScore = PlayerPrefs.GetInt("HighScore");
        _scoreTxt.text = highScore.ToString();
        _startPanel.SetActive(true);
        Time.timeScale = 0;
        _continue.SetActive(highScore > 0);
    }


    public void PlayButton()
    {
        Time.timeScale = 1;
        _startPanel.SetActive(false);
        PlayerPrefs.SetInt("HighScore",0);
        _scoreTxt.text = "0";
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        _startPanel.SetActive(false);
    }
    

    public void StopGame()
    {
        Time.timeScale = 0;
        _startPanel.SetActive(true);
    }
    public void SetScore(int value)
    {
        BoostedAction();
        score += value;
        _scoreTxt.text = score.ToString();
        ScaleBounce(_scoreTxt.transform);
    }

    private void BoostedAction()
    {
        boostedCount++;
        if (boostedCount > 0)
        {
            _boostedtxt.gameObject.SetActive(true);
            _boostedItem.gameObject.SetActive(true);
            _boostedtxt.text = boostedCount + "X";
            ScaleBounce(_boostedtxt.transform);
        }

        if (boostedCount <= 0)
        {
            _boostedtxt.gameObject.SetActive(false);
            _boostedItem.gameObject.SetActive(false);
        }

        if (boostedCount >= 10)
        {
            _boostedIcon.SetActive(true);
            _boostedItem.gameObject.SetActive(false);
            boostedCount = 0;
            _spawner.SetSpeed(boostedSpeed, true);
            StartCoroutine(BoostedCoroutine());
        }
    }

    private IEnumerator BoostedCoroutine()
    {
        yield return new WaitForSeconds(_boostedTimer);
        _spawner.SetSpeed(0, false);
        BoostedTurnOff();
    }

    public void SetHealth(int value)
    {
        health -= value;
        if (health <= 0) SaveData();
        _healthTxt.text = health.ToString();
        ScaleBounce(_healthTxt.transform);
        BoostedTurnOff();
    }

    private void BoostedTurnOff()
    {
        _boostedItem.gameObject.SetActive(false);
        _boostedtxt.gameObject.SetActive(false);
        _boostedIcon.SetActive(false);
        boostedCount = 0;
        StopCoroutine(BoostedCoroutine());
    }

    public void SaveData()
    {
        //StopGame();
        AudioManager.instance.PlayGameOverSound();
        var highScore=PlayerPrefs.GetInt("HighScore");
        if (score>highScore) PlayerPrefs.SetInt("HighScore",score);
        Invoke("LoadGame",0.5f);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(0);
    }
    public void ScaleBounce(Transform _transform)
    {
        _transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.3f)
            .SetEase(Ease.OutBack).OnComplete((() => _transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f)
                .SetEase(Ease.InOutSine)));
    }
}