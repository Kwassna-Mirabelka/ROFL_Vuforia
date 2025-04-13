using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class RuneConnectionManager : MonoBehaviour
{

    

    public GameObject[] runyLiczby;
    public GameObject[] runyZnaki;
    public bool[] areActiveLiczby;
    public bool[] areActiveZnaki;
    public float distToActivateZeZnaku = 2f;
    public float distToActivateZeWartosci = 2f;

    
    void Start()
    {
        distToActivateZeWartosci *= distToActivateZeWartosci;
        distToActivateZeZnaku *= distToActivateZeZnaku;
        runyLiczby = GameObject.FindGameObjectsWithTag("runeLiczba");
        runyZnaki = GameObject.FindGameObjectsWithTag("runeZnak");
        areActiveLiczby = new bool[runyLiczby.Length];
        areActiveZnaki = new bool[runyZnaki.Length];
        for (int i = 0; i < areActiveLiczby.Length; i++)
        {
            areActiveLiczby[i] = false;
        }
        for (int i = 0; i < areActiveZnaki.Length; i++)
        {
            areActiveZnaki[i] = false;
        }
    }

    private float distSQR = 0;

    // Update is called once per frame

    float tempMinDist = float.MaxValue;
    int minIndexZnak = -1;
    int minIndexSzansa =-1;
    int minIndexWartosc = -1;
    void Update()
    {
        for(int i=0; i < areActiveLiczby.Length; i++)
        {
            if (runyLiczby[i].GetComponentInChildren<Renderer>().enabled)
            {
                areActiveLiczby[i] = true;
            }
            else
            {
                areActiveLiczby[i] = false;
            }
        }
        for (int i = 0; i < areActiveZnaki.Length; i++)
        {
            if (runyZnaki[i].GetComponentInChildren<Renderer>().enabled)
            {
                areActiveZnaki[i] = true;
            }
            else
            {
                areActiveZnaki[i] = false;
            }
        }

        for (int i = 0; i < runyZnaki.Length; i++)
        {
            
            if (!areActiveZnaki[i])
            {
                continue;
            }
            tempMinDist = float.MaxValue;
            for (int j = 0; j < runyLiczby.Length; j++)
            {
                if (!areActiveLiczby[j])
                {
                    continue;
                }
                distSQR = Vector3.SqrMagnitude(runyZnaki[i].transform.position - runyLiczby[j].transform.position);
                if (distSQR < distToActivateZeWartosci)
                {
                    if(distSQR < tempMinDist)
                    {
                        tempMinDist = distSQR;
                        minIndexZnak = i;
                        minIndexSzansa = j;

                    }
                }
            }
        }

        if(minIndexSzansa == -1 || minIndexZnak==-1)
        {
            //sendZeroes
            return;
        }


        tempMinDist = float.MaxValue;
        for (int j = 0; j < runyLiczby.Length; j++)
        {
            if (j == minIndexSzansa)
            {
                continue;
            }
            if (!areActiveLiczby[j])
            {
                continue;
            }
            distSQR = Vector3.SqrMagnitude(runyLiczby[minIndexSzansa].transform.position - runyLiczby[j].transform.position);
            if (distSQR < distToActivateZeWartosci)
            {
                if (distSQR < tempMinDist)
                {
                    tempMinDist = distSQR;
                    minIndexWartosc = j;
                }
            }
        }
        if (minIndexWartosc == -1)
        {
            //sendZeroes
            return;
        }

        string figura = runyZnaki[minIndexZnak].GetComponent<ImageTargetBehaviour>().TargetName;
        string szansa = runyLiczby[minIndexSzansa].GetComponent<ImageTargetBehaviour>().TargetName;
        string wartosc = runyLiczby[minIndexWartosc].GetComponent<ImageTargetBehaviour>().TargetName;
        Debug.Log(figura + " " + szansa + " " + wartosc+" elo") ;

    }
}
