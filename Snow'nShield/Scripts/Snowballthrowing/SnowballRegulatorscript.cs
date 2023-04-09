using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballRegulatorscript : MonoBehaviour
{
    public static int nrOfSnowballs;
    [SerializeField] int maxNrOfSnowballs = 0;
    GameObject snowballToBeDeleted;

    // Start is called before the first frame update
    void Start()
    {
        if (maxNrOfSnowballs == 0)
            maxNrOfSnowballs = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if(nrOfSnowballs >= maxNrOfSnowballs)
        {
            snowballToBeDeleted = transform.GetChild(0).gameObject;
            Destroy(snowballToBeDeleted);
            nrOfSnowballs--;
        }
    }
}
