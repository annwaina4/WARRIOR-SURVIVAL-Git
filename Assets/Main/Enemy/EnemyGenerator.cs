using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retro.ThirdPersonCharacter
{
    public class EnemyGenerator : MonoBehaviour
    {
        //�G�̐�����
        public int enemycounter = 0;

        //�G�̔z��
        public GameObject[] enemyprefab = new GameObject[4];

        //�����ꏊ�̔z��
        public GameObject[] obeliskObject = new GameObject[4];
        
        //�G�̐����p�x
        float enemyTimecounter = 0f;
        float span = 3.0f;

        //�񕜃A�C�e���̐�����
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
                //�G�̐���
                if (enemyTimecounter > span)
                {
                    enemyTimecounter = 0f;

                    if (enemycounter <= 15)
                    {
                        //�ǂ̓G�𐶐����邩�̗���
                        int enemy = Random.Range(0, 4);

                        //�ǂ̏ꏊ�ɐ������邩�̗���
                        int respawn = Random.Range(0, 4);

                        Instantiate(enemyprefab[enemy], obeliskObject[respawn].transform.transform.position, Quaternion.identity);

                        enemycounter++;
                    }
                }

                //�񕜃A�C�e���̐���
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
            //Debug.Log("�񕜃A�C�e���̐��F" + healCounter);
            //Debug.Log("healTimeConter�F" + healTimeConter);
        }
    }
}
