using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Retro.ThirdPersonCharacter
{
    public class UIController : MonoBehaviour
    {
        private int score = 0;

        GameObject gameovertext;

        GameObject scoretext;
        
        GameObject finishText;
        //�������ԕ\��
        GameObject timetext;
        //public int countdownMinutes = 2;
        //private float countdownSeconds;
        private float countdownSeconds = 61f;
        void Start()
        {
            this.gameovertext = GameObject.Find("GameOverText");

            this.scoretext = GameObject.Find("ScoreText");
            
            this.finishText = GameObject.Find("Finish");

            this.timetext = GameObject.Find("TimeText");
            //this.countdownSeconds = countdownMinutes * 60;
        }

        void Update()
        {
            if (Movement.isEnd == false)
            {
                if (countdownSeconds > 0)
                {
                    //���Ԍ�������
                    countdownSeconds -= Time.deltaTime;
                    var span = new TimeSpan(0, 0, (int)countdownSeconds);
                    timetext.GetComponent<Text>().text = span.ToString(@"mm\:ss");
                }

                if (countdownSeconds <= 0)
                {
                    Movement.isEnd = true;
                    this.finishText.GetComponent<Text>().text = "FINISH";
                }
            }
            //Debug.Log("isEnd:" + Movement.isEnd);
        }

        //�X�R�A���Z
        public void AddScore(int value)
        {
            score += value;
            //UI�\��
            this.scoretext.GetComponent<Text>().text = "Score  " + this.score + "pt";
        }

        //�Q�[���I�[�o�[
        public void GameOver()
        {
            //UI�\��
            this.gameovertext.GetComponent<Text>().text = "GAME OVER";
        }
    }
}
