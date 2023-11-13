using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retro.ThirdPersonCharacter
{
    public class EnemyGenerator : MonoBehaviour
    {
        //敵の生成数
        public int enemycounter = 0;


        //敵の配列
        public GameObject[] enemyprefab = new GameObject[4];

        //生成場所の配列
        public GameObject[] obeliskObject = new GameObject[4];
        
        //敵の生成頻度
        float enemyTimecounter = 0f;
        float span = 3.0f;

        //回復アイテムの生成数
        public int healCounter = 1;

        float healTimeConter = 0f;

        public GameObject Healprefab;

        

        void Start()
        {
            
        }

        void Update()
        {
            enemyTimecounter += Time.deltaTime;
            healTimeConter += Time.deltaTime;

            if (Movement.isEnd == false)
            {
                if (enemyTimecounter > span)
                {
                    enemyTimecounter = 0f;

                    if (enemycounter <= 15)
                    {
                        //どの敵を生成するかの乱数
                        int enemy = Random.Range(0, 4);

                        //どの場所に生成するかの乱数
                        int respawn = Random.Range(0, 4);

                        Instantiate(enemyprefab[enemy], obeliskObject[respawn].transform.transform.position, Quaternion.identity);

                        enemycounter++;
                    }

                }

                if (healTimeConter > 15.0f)
                {
                    healTimeConter = 0f;

                    if (healCounter < 1)
                    {
                        GameObject healCopy = Instantiate(Healprefab, new Vector3(3.68f, -3.9f, -0.69f), Quaternion.identity);

                        healCounter++;
                    }
                }
            }

            //Debug.Log("回復アイテムの数：" + healCounter);
            //Debug.Log("healTimeConter：" + healTimeConter);

        }
    }
}
