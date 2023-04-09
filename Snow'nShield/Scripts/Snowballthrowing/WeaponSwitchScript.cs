using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponSwitchScript : MonoBehaviour
{
    [SerializeField] GameObject Gun;
    [SerializeField] GameObject Shield;
    bool weapon = true;
    bool canSwitch;
    AudioSource weaponSound;

    // Start is called before the first frame update
    void Start()
    {
        Gun.SetActive(false);
        canSwitch = true;
        weaponSound = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (ControllersManager.Instance.OnTriggerDown())
        {
            
            StartCoroutine(SwitchCoroutine());
        }

    }
    public void ForceToShield()
    {
        Gun.SetActive(false);
        Shield.SetActive(true);
        weapon = false;
    }
    IEnumerator SwitchCoroutine()
    {
        yield return new WaitForSeconds(.15f);
        if (weapon)
        {
            Gun.SetActive(true);
            Shield.SetActive(false);
            weapon = false;
            weaponSound.Play();
        }
        else
        {
            Gun.SetActive(false);
            Shield.SetActive(true);
            weapon = true;
            weaponSound.Play();

        }
        

    }

}
