using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(DestroyCoroutine());
    }
    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(.025f);
        spriteRenderer.color=new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a-.1f);
        if (spriteRenderer.color.a<=0)
            Destroy(gameObject);
        StartCoroutine(DestroyCoroutine());
    }
}
