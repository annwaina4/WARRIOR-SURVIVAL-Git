﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Retro.ThirdPersonCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Combat))]
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
    {
        private Animator _animator;
        private PlayerInput _playerInput;
        private Combat _combat;
        private CharacterController _characterController;

        private Vector2 lastMovementInput;
        private Vector3 moveDirection = Vector3.zero;

        public float gravity = 10;
        public float jumpSpeed = 4; 

        public float MaxSpeed = 10;
        private float DecelerationOnStop = 0.00f;

        private int maxHP = 8;

        private int nowHP;

        public Slider slider;

        //ゲーム終了
        static public bool isEnd = false;

        //回復アイテム
        GameObject healItem;
        public GameObject healEffectPrefab;

        //------------------------------------------------------------------------------------------------------------------
        //スタート
        //------------------------------------------------------------------------------------------------------------------
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
            _combat = GetComponent<Combat>();
            _characterController = GetComponent<CharacterController>();

            //Sliderを最大にする
            slider.value = 1;
            //HPを最大HPと同じ値に
            nowHP = maxHP;

            //回復アイテムを取得
            //this.healItem = GameObject.Find("Heal"); 
        }

        //------------------------------------------------------------------------------------------------------------------
        //アップデート
        //------------------------------------------------------------------------------------------------------------------
        private void Update()
        {            
            //if (_animator == null) return;

            //ゲームオーバー
            if (isEnd)
            {
                //リスタート処理
                // クリックされたらシーンをロードする
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    //static変数の初期化
                    isEnd = false;
                    //SampleSceneを読み込む
                    SceneManager.LoadScene("title");                    
                }
                //アニメーション　待機
                _animator.SetFloat("InputX", 0);
                _animator.SetFloat("InputY", 0);
            }

            else
            {
                if (_combat.AttackInProgress)
                {
                    StopMovementOnAttack();
                }
                else
                {
                    Move();
                }
            }
        }

        private void Move()
        {
            var x = _playerInput.MovementInput.x;
            var y = _playerInput.MovementInput.y;

            bool grounded = _characterController.isGrounded;

            if (grounded)
            {
                moveDirection = new Vector3(x, 0, y);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= MaxSpeed;
                if (_playerInput.JumpInput)
                    moveDirection.y = jumpSpeed;
            }

            moveDirection.y -= gravity * Time.deltaTime;
            _characterController.Move(moveDirection * Time.deltaTime);

            _animator.SetFloat("InputX", x);
            _animator.SetFloat("InputY", y);
            _animator.SetBool("IsInAir", !grounded);
        }

        private void StopMovementOnAttack()
        {
            var temp = lastMovementInput;
            temp.x -= DecelerationOnStop;
            temp.y -= DecelerationOnStop;
            lastMovementInput = temp;

            _animator.SetFloat("InputX", lastMovementInput.x);
            _animator.SetFloat("InputY", lastMovementInput.y);
        }

        //------------------------------------------------------------------------------------------------------------------
        //衝突判定
        //------------------------------------------------------------------------------------------------------------------
        private void OnTriggerEnter(Collider other)
        {
            //敵の攻撃
            if(other.gameObject.tag=="Attack"&&nowHP>0)
            {
                GetComponent<Animator>().SetTrigger("damage");

                //ダメージの割り込みでAttackInProgressがtrueのままになるので強制解除
                _combat.AttackInProgressClear();

                nowHP--;

                //HPをSliderに反映
                slider.value = (float)nowHP / (float)maxHP;
            }

            //回復アイテムの取得
            if (other.gameObject.tag == "Heal" )
            {
                nowHP =maxHP;
                //HPをSliderに反映
                slider.value = (float)nowHP / (float)maxHP;

                Destroy(other.gameObject);

                GameObject healEffect = Instantiate(healEffectPrefab, this.transform.position, Quaternion.identity);                
                Destroy(healEffect, 0.5f);

                //回復アイテムのカウントを減らす
                GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>().healCounter--;
            }

            //死亡
            if (nowHP<=0)
            {
                GetComponent<Animator>().SetBool("death", true);

                //CanvasにGAMEOVERを表示
                GameObject.Find("Canvas").GetComponent<UIController>().GameOver();

                //ゲームオーバー
                isEnd = true;
            }
        }
    }
}