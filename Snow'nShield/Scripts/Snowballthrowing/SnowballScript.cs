using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballScript : MonoBehaviour
{
    float Dmg;
    public float speed;
    public bool IsEnemyProj = true;
    public float lifespan = 7;
    
    [SerializeField] public GameObject player;
    [SerializeField] SMenuButtons sgManager;
    [SerializeField] GameObject splashEffect;
    //deletes any snowballs that weren't deleted
    IEnumerator DeleteCoroutine()
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(DeleteCoroutine());
        if (IsEnemyProj)
        {
            GetComponent<Rigidbody>().mass += .6f;
            GetComponent<Rigidbody>().drag += .6f;
            GetComponent<Rigidbody>().angularDrag += .6f;
        }

    }
    private void FixedUpdate()
    {
        float step = speed *20* Time.deltaTime; 
        if (IsEnemyProj)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }

    }
    public void SetAllegiance(bool allegiance)
    {
        IsEnemyProj = allegiance;
    }
    public void SetPlayerTarget(GameObject pl)
    {
        player = pl;
    }
    public void SetDamage(float d)
    {
        Dmg = d;
    }
    public void SetGenerator(SMenuButtons hthbr)
    {
        sgManager = hthbr;
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (IsEnemyProj)
                {
                    sgManager.PlayerDamage(Dmg);
                    Instantiate(splashEffect, transform.position, splashEffect.transform.rotation).transform.LookAt(player.transform);
                    Destroy(gameObject);
                }
                else
                    Physics.IgnoreCollision(this.GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>(), true);
                break;

            case "Enemy":
                if (!IsEnemyProj)
                {
                    sgManager.AddToScore(3);
                    sgManager.EnemyDamage(Dmg);
                    Instantiate(splashEffect, transform.position, splashEffect.transform.rotation);
                    Destroy(gameObject);
                }
                else
                    Physics.IgnoreCollision(this.GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>(), true);
                break;

            case "Shield":
                sgManager.AddToScore(5);
                Instantiate(splashEffect, transform.position, splashEffect.transform.rotation);
                Destroy(gameObject);
                break;

            case "SnowGButton":
                SnowGunButton button = collision.gameObject.GetComponent<SnowGunButton>();
                if (button != null)
                    sgManager.StartDifficulty(button.type.ToString());
                break;
            default:
                break;
        }
        

    }
}
