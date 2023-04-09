using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeginingCountdown : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] int countdown;
    CannonManager CanonManager;
    SnowballGenerator Snowman;
    [SerializeField] WeaponSwitchScript WeaponSwitcher;


    // Start is called before the first frame update
    void OnEnable()
    {
        Snowman.enabled = false;
        CanonManager.enabled = false;
        WeaponSwitcher.enabled = false;
        text = GetComponent<TextMeshProUGUI>();
        text.text = countdown.ToString();
        
        StartCoroutine(CountdownCoroutine(countdown));
    }
    public void SetEnemies(CannonManager canonmanager, SnowballGenerator snowman)
    {
        CanonManager=canonmanager;
        Snowman=snowman;
    }

   
    void OnCountdownCompleted()
    {
        CanonManager.enabled = true;
        WeaponSwitcher.enabled = true;
        Snowman.StartThrowingAutomatically();
        gameObject.SetActive(false);

    }
    IEnumerator CountdownCoroutine(int seconds)
    {
        for(int i = seconds; i > 0; i--)
        {
            text.text = i.ToString();
            yield return new WaitForSeconds(1);  
        }

        OnCountdownCompleted();
    }
  
}
