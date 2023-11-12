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
        void Start()
        {
            this.gameovertext = GameObject.Find("GameOverText");
            this.scoretext = GameObject.Find("ScoreText");
        }


        void Update()
        {

        }

        //スコア加算
        public void AddScore(int value)
        {
            score += value;
            //UI表示
            this.scoretext.GetComponent<Text>().text = "Score  " + this.score + "pt";

        }

        public void GameOver()
        {
            //UI表示
            this.gameovertext.GetComponent<Text>().text = "GAME OVER";
        }
    }
}
