using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SMenuButtons : SceneController
{
    [Header("Score")]
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI RealTimeScore;
    int ScorePenalty;
    int score = 100;
    [Space(5)]
    [Header("Menu Navigation")]
    [SerializeField] GameObject MainMenuButtons;
    [SerializeField] GameObject DifficultyButtons;
    [SerializeField] SnowballRegulatorscript SRegulator;
    [SerializeField] GameObject EmergencyExitButton;
    [SerializeField] GameObject MenuTerrain;

    [Space(5)]

    [Header("Levels")]
    [SerializeField] LevelDataScript EasyMode;
    [SerializeField] LevelDataScript MediumMode;
    [SerializeField] LevelDataScript HardMode;
    [Space(5)]

    [Header("Healthbars")]
    [SerializeField] RawImage playerBar;
    RawImage enemyBar;
    float PlayerHP;
    float PlayerMaxHP = 1000;
    float EnemyHP;
    float EnemyMaxHP = 1000;
    [Space(5)]

    [Header("Game over variables")]
    [SerializeField] Canvas ButtonsCanvas;
    [SerializeField] GameObject HealthbarsCanvas;
    [SerializeField] GameObject WeaponSwitcher;
    [SerializeField] GameObject GameWonButtons;
    [SerializeField] GameObject GameLossButtons;

    [Header("Sound Effects")]
    [SerializeField] MusicBoxScript musicBox;
    [SerializeField] AudioClip MenuMusic;
    [SerializeField] AudioClip EasyMusic;
    [SerializeField] AudioClip MediumMusic;
    [SerializeField] AudioClip HardMusic;

    [SerializeField] GameObject Player;
    [SerializeField] BeginingCountdown Countdown;
    [Space(5)]

    SnowballGenerator snowman;
    CannonManager cannonManager;
    SnowmanMovementScript snowmanMovement;

    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        musicBox.playMusic(MenuMusic);
        PlayerHP = PlayerMaxHP;
        EnemyHP = EnemyMaxHP;
        HealthbarsCanvas.SetActive(false);
        DifficultyButtons.SetActive(false);
        EasyMode.gameObject.SetActive(false);
        MediumMode.gameObject.SetActive(false);
        HardMode.gameObject.SetActive(false);
        EmergencyExitButton.gameObject.SetActive(false);
        
    }
    //Level difficulties
    public void MoveToDifficulties()
    {
        WeaponSwitcher.SetActive(true);
        DifficultyButtons.SetActive(true);
        MainMenuButtons.SetActive(false);
        WeaponSwitcher.GetComponent<WeaponSwitchScript>().ForceToShield();
    }
    void StartGame(LevelDataScript levelPackage)
    {
        ScorePenalty=levelPackage.ScorePenalty;
        DifficultyButtons.SetActive(false);
        Countdown.SetEnemies(levelPackage.CannonManager, levelPackage.Snowman);
        levelPackage.gameObject.SetActive(true);
        Player.transform.position = levelPackage.transform.position;
        HealthbarsCanvas.SetActive(true);
        Countdown.gameObject.SetActive(true);
        snowman = levelPackage.Snowman;
        cannonManager = levelPackage.CannonManager;
        enemyBar = levelPackage.SnowmanHealthbar;
        snowmanMovement = levelPackage.SnowmanMovementScript;
        WeaponSwitcher.GetComponent<WeaponSwitchScript>().ForceToShield();
        WeaponSwitcher.SetActive(true);
        HealthbarsCanvas.SetActive(true);
        EmergencyExitButton.gameObject.SetActive(true);
    }
    //Healthbars
    public void PlayerDamage(float hp)
    {
        PlayerHP -= hp;

        score -= ScorePenalty;
        if(score < 0)
        {
            score = 0;
        }
        RealTimeScore.text = score.ToString();
        UpdateBar(PlayerHP, playerBar);
        if (PlayerHP <= 0 && EnemyHP>0)
        {
            ShowGameOverMenu();
            GameLossButtons.gameObject.SetActive(true);
            
        }

    }
    public void EnemyDamage(float hp)
    {
        EnemyHP -= hp;
        snowmanMovement.HitEffect();
        UpdateBar(EnemyHP, enemyBar);       
        if (EnemyHP <= 0)
        {
            ScoreText.text = score.ToString();
            ShowGameOverMenu();
            GameWonButtons.gameObject.SetActive(true);
        }
        snowmanMovement.SetDespPhase(EnemyHP);


    }
    void transitionToLevel(LevelDataScript level,AudioClip clip)
    {
        musicBox.StopPlaying();
        screenFade.FadeOut();
        StartGame(level);
        musicBox.playMusic(clip);
        ResetHealthbars();
        screenFade.FadeIn();
    }
    void ShowGameOverMenu()
    {

        musicBox.StopPlaying();
        ButtonsCanvas.gameObject.SetActive(true);
        WeaponSwitcher.gameObject.SetActive(false);
        snowman.StopThrowing();
        snowman.gameObject.SetActive(false);
        cannonManager.enabled = false;
        HealthbarsCanvas.SetActive(false);
        ResetHealthbars();
        foreach(Transform snow in SRegulator.transform)
        {
            Destroy(snow.gameObject);
        }
    }
    public void SetHealthbars(bool set)
    {
        playerBar.enabled=set;
        enemyBar.enabled=set;
    }
    void UpdateBar(float hp, RawImage img)
    {
        img.rectTransform.sizeDelta = new Vector2(hp * .6f, img.rectTransform.sizeDelta.y);

    }
    public void ResetHealthbars()
    {
        PlayerHP = PlayerMaxHP;
        EnemyHP = EnemyMaxHP;
        UpdateBar(PlayerHP, playerBar);
        UpdateBar(EnemyHP, enemyBar);
    }
    //Button Functions
    public void StartDifficulty(string difficulty)
    {
        MenuTerrain.SetActive(false);
        switch (difficulty)
        {
            default:
            case "easy":
                transitionToLevel(EasyMode, EasyMusic);
                break;
            case "medium":
                transitionToLevel(MediumMode, MediumMusic);
                break;
            case "hard":
                transitionToLevel(HardMode, HardMusic);
                break;

        }
    }

    public void BackToMenu()
    {
        score = 100;
        ScoreText.text = score.ToString();
        screenFade.FadeOut();
        musicBox.StopPlaying();
        MenuTerrain.SetActive(true);
        GameWonButtons.SetActive(false);
        GameLossButtons.SetActive(false);
        HealthbarsCanvas.SetActive(false);
        snowman.gameObject.SetActive(true);
        Player.transform.position = Vector3.zero;
        MainMenuButtons.SetActive(true);
        EasyMode.gameObject.SetActive(false);
        MediumMode.gameObject.SetActive(false);
        HardMode.gameObject.SetActive(false);
        musicBox.playMusic(MenuMusic);
        EmergencyExitButton.gameObject.SetActive(false);
        screenFade.FadeIn();
    }
    public void TryAgain()
    {
        score = 100;
        ScoreText.text = score.ToString();
        screenFade.FadeOut();
        musicBox.playMusic(musicBox.GetCurrentAudio());
        HealthbarsCanvas.gameObject.SetActive(true);
        WeaponSwitcher.gameObject.SetActive(true);
        HealthbarsCanvas.SetActive(true);
        ButtonsCanvas.gameObject.SetActive(false);
        snowman.gameObject.SetActive(true);
        Countdown.gameObject.SetActive(true);
        screenFade.FadeIn();

    }
    public void AddToScore(int s)
    {
        score += s;
        RealTimeScore.text = score.ToString();
    }
}
