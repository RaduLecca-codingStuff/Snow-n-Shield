using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanMovementScript : MonoBehaviour
{

    [SerializeField] float startDistance;
    [SerializeField] float startSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] float maxSpeed;
    [SerializeField] float HpAtPhase2;
    [SerializeField] float maxAnimationSpeed;
    [SerializeField] GameObject Player;

    bool isRight;
    bool facePlayer = false;

    //variables used for variations
    float distance;
    float speed;


    Vector3 firstpos;
    Rigidbody rb;
    SnowballGenerator sg;
    Animator animator;
    SkinnedMeshRenderer skinnedMeshRenderer;
    float animspeed;
    float addAnimSpeed;
    void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        rb = GetComponentInChildren<Rigidbody>();
        sg = GetComponentInChildren<SnowballGenerator>();
        firstpos = transform.position;
        animator = GetComponentInChildren<Animator>();
        speed = startSpeed;
        distance = startDistance;
        animspeed = animator.speed;
        addAnimSpeed = 0;
    }
    void OnEnable()
    {
        firstpos = transform.position;
        StartCoroutine(MovementCoroutine());
        if (Random.Range(0, 2) == 1)
            isRight = true;
        else
            isRight = false;

    }
    private void Update()
    {
        if (facePlayer && speed > 0)
        {
            rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, Player.transform.rotation, 0.1f);
        }

    }
    IEnumerator MovementCoroutine()
    {
        yield return new WaitForSeconds(.05f);

        animator.speed = 1+ (addAnimSpeed * maxAnimationSpeed * .1f);

        if (transform.position.x >= firstpos.x + distance)
            isRight = false;
        if (transform.position.x <= firstpos.x - distance)
            isRight = true;

        if (isRight && speed>0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(firstpos.x + distance, firstpos.y, firstpos.z), speed * Time.deltaTime);
            if (rb.transform.localEulerAngles.y !=270)
            {
                rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, Quaternion.Euler(0,270, 0), 0.4f);
            }
        }
        else if (!isRight && speed>0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(firstpos.x - distance, firstpos.y, firstpos.z), speed * Time.deltaTime);
            if (rb.transform.localEulerAngles.y !=90)
            {
                rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, Quaternion.Euler(0, 90, 0), 0.4f);
            }
        }
        else
        {
            rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, Player.transform.rotation, 0.1f);
        }


        if (sg.GetIfThrows())
        {
            animator.speed = animspeed;
            facePlayer = true;
            animator.SetBool("isThrowing", true);
            yield return new WaitForSeconds(sg.GetThrowDelay() / 3);
            facePlayer = false;
            animator.SetBool("isThrowing", false);
        }

        StartCoroutine(MovementCoroutine());

    }
    IEnumerator HitColorEffect()
    {
        skinnedMeshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        while (skinnedMeshRenderer.material.color.g<1)
        {
            skinnedMeshRenderer.material.color += new Color(0, .3f, .3f);
            yield return new WaitForSeconds(.1f);
 
        }
    }

    public void HitEffect()
    {
        StartCoroutine(HitColorEffect());
    }
    private void OnDisable()
    {
        //resets everything about the snowman that's part of the code
        StopAllCoroutines();
        rb.velocity = Vector3.zero;
        transform.position = firstpos;
        skinnedMeshRenderer.material.color = Color.white;
        distance = startDistance;
        speed = startSpeed;
    }
    public void SetDespPhase(float hp)
    {
        if (hp <= HpAtPhase2)
        {
            if (speed < maxSpeed)
                speed++;
            if (distance < maxDistance)
                distance++;
            if (animspeed + (addAnimSpeed * .3f) < maxAnimationSpeed)
                addAnimSpeed += 0.2f;
        }
    }
}
