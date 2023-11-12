using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    //“G‚Ì¶¬”
    public int enemycounter = 0;

    //“G‚Ì”z—ñ
    public GameObject[] enemyprefab = new GameObject[4];

    //¶¬êŠ‚Ì”z—ñ
    public GameObject[] obeliskObject = new GameObject[4];

    float timecounter = 0f;
    float span = 3.0f;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        timecounter += Time.deltaTime;

        if (timecounter > span)
        {
            timecounter = 0f;

            if (enemycounter <= 10)
            {
                //‚Ç‚Ì“G‚ğ¶¬‚·‚é‚©‚Ì—”
                int enemy = Random.Range(0, 4);

                //‚Ç‚ÌêŠ‚É¶¬‚·‚é‚©‚Ì—”
                int respawn = Random.Range(0, 4);

                Instantiate(enemyprefab[enemy],obeliskObject[respawn].transform.transform.position,Quaternion.identity);

                enemycounter++;
            }

        }
 

    }
}
