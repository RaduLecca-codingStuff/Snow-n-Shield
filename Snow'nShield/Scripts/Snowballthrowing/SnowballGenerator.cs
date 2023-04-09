using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballGenerator : MonoBehaviour
{
    [Header("General variables")]
    [SerializeField] GameObject snowballPrefab;
    [SerializeField] bool isEnemyProjectile;
    [SerializeField] bool itThrowsPeriodically;
    [SerializeField] float timeUntilThrow;
    [SerializeField] float damage;
    [SerializeField] GameObject PlayerCollider;
    [SerializeField] GameObject regulator;
    [SerializeField] SMenuButtons snowGameManager;
    [Space(10)]
    [Header("Audio")]
    [SerializeField] AudioClip[] FireClips;
    AudioSource audioSource;
    [Space(10)]
    [Header("Enemy Only")]
    [SerializeField] float enemyMaxThrowHeight=2;

    public float speed;
    bool isThrowing;
    float trackRandom=0;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isThrowing = false;
    }

    // Start is called before the first frame update
    void OnEnable()
    {

        snowballPrefab.GetComponent<SnowballScript>().IsEnemyProj = isEnemyProjectile;

        if (isEnemyProjectile)
        {
            snowballPrefab.GetComponent<SnowballScript>().SetPlayerTarget(PlayerCollider);
        }
        //Snowgun only code
        if (itThrowsPeriodically && !isEnemyProjectile)
        {
            StartCoroutine(ThrowCoroutine());
        }

    }
    IEnumerator ThrowCoroutine()
    {
        isThrowing = false;
        yield return new WaitForSeconds((timeUntilThrow / 4) * 3);
        isThrowing = true;
        yield return new WaitForSeconds(timeUntilThrow / 4);
        ThrowAction();
        StartCoroutine(ThrowCoroutine());

    }
    public void ThrowAction()
    {
        int r;
        if (FireClips.Length != 0)
        {
            r = Random.Range(1, FireClips.Length);
            audioSource.clip = FireClips[r];
        }

        if (regulator != null)
        {
            audioSource.Play();
            GameObject snowball = Instantiate(snowballPrefab, transform.position, snowballPrefab.transform.rotation, regulator.transform);
            snowball.GetComponent<SnowballScript>().SetDamage(damage);
            snowball.GetComponent<SnowballScript>().SetGenerator(snowGameManager);
            snowball.GetComponent<SnowballScript>().SetPlayerTarget(PlayerCollider);
            snowball.GetComponent<SnowballScript>().IsEnemyProj = isEnemyProjectile;

            if (isEnemyProjectile)
            {

                float r2 = Random.Range(1, enemyMaxThrowHeight);
                if (r2 == trackRandom)
                {
                    if (r2 < enemyMaxThrowHeight/2 )
                        r2 = Random.Range(trackRandom+1, enemyMaxThrowHeight);
                    else
                        r2 = Random.Range(2, trackRandom-1);

                }
                snowball.transform.LookAt(PlayerCollider.transform.position);
                snowball.GetComponent<Rigidbody>().AddForce((snowball.transform.forward + new Vector3(0, r2 * 0.1f, 0)) * 0.1f * speed, ForceMode.Impulse);
                trackRandom = r2;
            }
            else
            {
                snowball.transform.forward = transform.forward;
                snowball.GetComponent<Rigidbody>().AddForce((snowball.transform.forward) * 0.15f * speed, ForceMode.Impulse);
            }

        }

    }

    public void SetRegulator(GameObject gme)
    {
        regulator = gme;
    }
    public void StopThrowing()
    {
        StopAllCoroutines();
    }
    public void StartThrowingAutomatically()
    {
        if (itThrowsPeriodically)
        {
            StartCoroutine(ThrowCoroutine());
        }
    }
    public bool GetIfThrows()
    {
        return isThrowing;
    }
    public float GetThrowDelay()
    {
        return timeUntilThrow;
    }
    
}
