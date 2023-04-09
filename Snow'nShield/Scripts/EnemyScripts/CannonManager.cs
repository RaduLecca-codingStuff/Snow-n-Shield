using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    [SerializeField] float SwapDelay;
    SnowballGenerator[] CannonArray;
    Color initCol;

    // Start is called before the first frame update
    private void OnEnable()
    {
        CannonArray = transform.GetComponentsInChildren<SnowballGenerator>();
        StartCoroutine(SwitchCoroutine());
        MeshRenderer gener = CannonArray[1].gameObject.GetComponent<MeshRenderer>();
        initCol = gener.material.color;

    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator SwitchCoroutine()
    {

        yield return new WaitForSeconds(1);
        int cIndex = Random.Range(0, transform.childCount);
        MeshRenderer generator = CannonArray[cIndex].gameObject.GetComponent<MeshRenderer>();
        initCol = generator.material.color;

        for (int i = 0; i < 3; i++)
        {
            generator.material.SetColor("_Color", initCol + new Color(0, 1, 1));
            yield return new WaitForSeconds(SwapDelay / 6);
            generator.material.SetColor("_Color", initCol + new Color(0, .3f, .3f));
            yield return new WaitForSeconds(SwapDelay / 6);
        }

        yield return new WaitForSeconds(SwapDelay / 6);
        CannonArray[cIndex].ThrowAction();
        generator.material.SetColor("_Color", initCol);
        yield return new WaitForSeconds(SwapDelay);
        StartCoroutine(SwitchCoroutine());
    }
    public void SetCannonsBack()
    {
        for (int i = 0; i < 3; i++)
        {
            CannonArray[i].gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", initCol);
        }
    }

}
