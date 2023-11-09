using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    //状態（ステートパターン）
    private int stateNumber = 0;

    //汎用タイマー
    private float timeCounter = 0f;

    private Animator myanimator;

    private Rigidbody myRigidbody;

    private GameObject player;

    private int maxHP = 3;

    private int nowHP;

    public Slider slider;

    //攻撃用
    public GameObject attackPrefab;
    private GameObject childobj;
    public GameObject effectPrefab;

    //------------------------------------------------------------------------------------------------------------------
    //スタート
    //------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        this.myanimator = GetComponent<Animator>();

        this.myRigidbody = GetComponent<Rigidbody>();

        this.player = GameObject.Find("Player");

        childobj = transform.Find("skillPoint").gameObject;

        //Sliderを最大にする
        slider.value = 1;
        //HPを最大HPと同じ値に
        nowHP = maxHP;
    }

    //------------------------------------------------------------------------------------------------------------------
    //オリジナル関数
    //------------------------------------------------------------------------------------------------------------------

    //距離を求める
    float getLength(Vector3 current, Vector3 target)
    {
        return Mathf.Sqrt(((current.x - target.x) * (current.x - target.x)) + ((current.z - target.z) * (current.z - target.z)));
    }

    //方向を求める ※オイラー（-180～0～+180)
    float getEulerAngle(Vector3 current, Vector3 target)
    {
        Vector3 value = target - current;
        return Mathf.Atan2(value.x, value.z) * Mathf.Rad2Deg; //ラジアン→オイラー
    }

    //方向を求める ※ラジアン
    float getRadian(Vector3 current, Vector3 target)
    {
        Vector3 value = target - current;
        return Mathf.Atan2(value.x, value.z);
    }

    //------------------------------------------------------------------------------------------------------------------
    //アップデート
    //------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        //タイマー加算
        timeCounter += Time.deltaTime;

        //方向を求める
        float direction = getEulerAngle(this.transform.position, player.transform.position);

        //距離を求める
        float length = getLength(this.transform.position, player.transform.position);

        //**************************************************************************************************************
        //ここから状態処理
        //**************************************************************************************************************

        //待機
        if (stateNumber == 0)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1秒経過
            if (timeCounter > 1.0f)
            {
                //タイマーリセット
                timeCounter = 0f;

                // アニメーション　前進
                this.myanimator.SetFloat("speed", 1.0f);

                //状態の遷移（前進）
                stateNumber = 1;
            }
            //プレーヤーが近い時
            else if (length < 2.0f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //アニメーション　攻撃
                this.myanimator.SetTrigger("attack");

                //状態の遷移（攻撃）
                stateNumber = 2;
            }

        }

        //前進
        else if (stateNumber == 1)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //移動
            myRigidbody.velocity = transform.forward * 2.5f;

            //5秒経過
            if (timeCounter > 5.0f)
            {
                timeCounter = 0f;

                //アニメーション　待機
                this.myanimator.SetFloat("speed", 0);

                //状態の遷移（待機）
                stateNumber = 0;
            }
            //プレーヤーが近い時
            else if (length < 2.0f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //アニメーション　待機
                this.myanimator.SetFloat("speed", 0);

                //アニメーション　攻撃
                this.myanimator.SetTrigger("attack");

                //状態の遷移（攻撃
                stateNumber = 2;
            }
        }

        else if (stateNumber == 2)
        {
            //攻撃モーション終わり
            if (timeCounter > 1.2f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //状態の遷移（待機）
                stateNumber = 0;
            }
        }

    }

    //------------------------------------------------------------------------------------------------------------------
    //攻撃イベント
    //------------------------------------------------------------------------------------------------------------------

    public void AttackEvent()
    {
        //Debug.Log("AttackEvent");

        //攻撃の生成
        GameObject attack = Instantiate(attackPrefab, childobj.transform.position, this.transform.rotation);
        Destroy(attack.gameObject, 0.2f);
        GameObject effect = Instantiate(effectPrefab, childobj.transform.position, this.transform.rotation);
        Destroy(effect.gameObject, 0.5f);
    }

    //------------------------------------------------------------------------------------------------------------------
    //衝突判定
    //------------------------------------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log("OnCollisionEnter:" + other.gameObject.tag);

        //ファイアーボール
        if (other.gameObject.tag == "Fireball" && nowHP > 0)
        {
            this.myanimator.SetTrigger("damage");

            nowHP--;

            //HPをSliderに反映
            slider.value = (float)nowHP / (float)maxHP;
        }

        //メテオ
        if (other.gameObject.tag == "Meteor" && nowHP > 0)
        {
            this.myanimator.SetTrigger("damage");

            nowHP -= 3;

            //HPをSliderに反映
            slider.value = (float)nowHP / (float)maxHP;
        }

        //Debug.Log("残りHP" + this.HP);

        if (nowHP <= 0)
        {
            this.myanimator.SetBool("death", true);

            //ステートパターンを停止
            stateNumber = -1;

            //自由落下を停止
            myRigidbody.useGravity = false;
            //衝突をなくす
            GetComponent<CapsuleCollider>().enabled = false;

            //3秒後に破棄
            Destroy(this.gameObject, 3.0f);
        }
    }
}
