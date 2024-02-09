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
        //制限時間表示
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
                    //時間減少処理
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

        //スコア加算
        public void AddScore(int value)
        {
            score += value;
            //UI表示
            this.scoretext.GetComponent<Text>().text = "Score  " + this.score + "pt";
        }

        //ゲームオーバー
        public void GameOver()
        {
            //UI表示
            this.gameovertext.GetComponent<Text>().text = "GAME OVER";
        }
    }
}
