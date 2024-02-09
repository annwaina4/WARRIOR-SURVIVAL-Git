using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Retro.ThirdPersonCharacter
{
    public class MigoController : MonoBehaviour
    {
        //��ԁi�X�e�[�g�p�^�[���j
        private int stateNumber = 0;

        //�ėp�^�C�}�[
        private float timeCounter = 0f;

        private Animator myanimator;

        private Rigidbody myRigidbody;

        private GameObject player;

        private int maxHP = 9;

        private int nowHP;

        public Slider slider;

        //�U���p
        public GameObject attackPrefab;
        private GameObject childobj;
        public GameObject effectPrefab;

        //------------------------------------------------------------------------------------------------------------------
        //�X�^�[�g
        //------------------------------------------------------------------------------------------------------------------
        void Start()
        {
            this.myanimator = GetComponent<Animator>();

            this.myRigidbody = GetComponent<Rigidbody>();

            this.player = GameObject.Find("Player");

            childobj = transform.Find("skillPoint").gameObject;

            //Slider���ő�ɂ���
            slider.value = 1;
            //HP���ő�HP�Ɠ����l��
            nowHP = maxHP;
        }

        //------------------------------------------------------------------------------------------------------------------
        //�����ƕ��������߂�֐�
        //------------------------------------------------------------------------------------------------------------------

        //���������߂�
        float getLength(Vector3 current, Vector3 target)
        {
            return Mathf.Sqrt(((current.x - target.x) * (current.x - target.x)) + ((current.z - target.z) * (current.z - target.z)));
        }

        //���������߂� ���I�C���[�i-180�`0�`+180)
        float getEulerAngle(Vector3 current, Vector3 target)
        {
            Vector3 value = target - current;
            return Mathf.Atan2(value.x, value.z) * Mathf.Rad2Deg; //���W�A�����I�C���[
        }

        //���������߂� �����W�A��
        float getRadian(Vector3 current, Vector3 target)
        {
            Vector3 value = target - current;
            return Mathf.Atan2(value.x, value.z);
        }

        //------------------------------------------------------------------------------------------------------------------
        //�A�b�v�f�[�g
        //------------------------------------------------------------------------------------------------------------------
        void Update()
        {
            //�^�C�}�[���Z
            timeCounter += Time.deltaTime;

            //���������߂�
            float direction = getEulerAngle(this.transform.position, player.transform.position);

            //���������߂�
            float length = getLength(this.transform.position, player.transform.position);

            //**************************************************************************************************************
            //���������ԏ���
            //**************************************************************************************************************

            //�ҋ@
            if (stateNumber == 0)
            {
                //�v���[���[�̕���������
                this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

                //1�b�o��
                if (timeCounter > 1.0f)
                {
                    //�^�C�}�[���Z�b�g
                    timeCounter = 0f;

                    // �A�j���[�V�����@�O�i
                    this.myanimator.SetFloat("speed", 1.0f);

                    //��Ԃ̑J�ځi�O�i�j
                    stateNumber = 1;
                }
                //�v���[���[���߂���
                else if (length < 2.0f)
                {
                    //�^�C�}�[���Z�b�g
                    timeCounter = 0f;

                    //�A�j���[�V�����@�U��
                    this.myanimator.SetTrigger("attack");

                    //��Ԃ̑J�ځi�U���j
                    stateNumber = 2;
                }
            }

            //�O�i
            else if (stateNumber == 1)
            {
                //�v���[���[�̕���������
                this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

                //�ړ�
                myRigidbody.velocity = transform.forward * 4.5f;

                //5�b�o��
                if (timeCounter > 5.0f)
                {
                    timeCounter = 0f;

                    //�A�j���[�V�����@�ҋ@
                    this.myanimator.SetFloat("speed", 0);

                    //��Ԃ̑J�ځi�ҋ@�j
                    stateNumber = 0;
                }
                //�v���[���[���߂���
                else if (length < 2.0f)
                {
                    //�^�C�}�[���Z�b�g
                    timeCounter = 0f;

                    //�A�j���[�V�����@�ҋ@
                    this.myanimator.SetFloat("speed", 0);

                    //�A�j���[�V�����@�U��
                    this.myanimator.SetTrigger("attack");

                    //��Ԃ̑J�ځi�U���j
                    stateNumber = 2;
                }
            }

            //�U��
            else if (stateNumber == 2)
            {
                //�U�����[�V�����I���
                if (timeCounter > 1.2f)
                {
                    //�^�C�}�[���Z�b�g
                    timeCounter = 0f;

                    //��Ԃ̑J�ځi�ҋ@�j
                    stateNumber = 0;
                }
            }

            //**************************************************************************************************************
            //�Q�[���]�I�[�o�[�Ď�
            //**************************************************************************************************************

            if (Movement.isEnd)
            {
                //�A�j���[�V�����@�ҋ@
                this.myanimator.SetFloat("speed", 0);

                //�X�e�[�g�p�^�[�����~
                stateNumber = -1;
            }
        }

        //------------------------------------------------------------------------------------------------------------------
        //�U���C�x���g
        //------------------------------------------------------------------------------------------------------------------
        public void AttackEvent()
        {
            //Debug.Log("AttackEvent");
            //�U���̐���
            GameObject attack = Instantiate(attackPrefab, childobj.transform.position, this.transform.rotation);
            Destroy(attack.gameObject, 0.2f);
            GameObject effect = Instantiate(effectPrefab, childobj.transform.position, this.transform.rotation);
            Destroy(effect.gameObject, 0.5f);
        }

        //------------------------------------------------------------------------------------------------------------------
        //�Փ˔���
        //------------------------------------------------------------------------------------------------------------------
        private void OnCollisionEnter(Collision other)
        {
            //Debug.Log("OnCollisionEnter:" + other.gameObject.tag);

            //�t�@�C�A�[�{�[��
            if (other.gameObject.tag == "Fireball" && nowHP > 0)
            {
                this.myanimator.SetTrigger("damage");

                nowHP-=4;

                timeCounter = 0f;

                //��Ԃ̑J�ځi�ҋ@�j
                stateNumber = 0;

                //HP��Slider�ɔ��f
                slider.value = (float)nowHP / (float)maxHP;
            }

            //���e�I
            if (other.gameObject.tag == "Meteor" && nowHP > 0)
            {
                this.myanimator.SetTrigger("damage");

                nowHP --;

                //HP��Slider�ɔ��f
                slider.value = (float)nowHP / (float)maxHP;
            }

            //���S
            if (nowHP <= 0)
            {
                this.myanimator.SetBool("death", true);

                //�X�e�[�g�p�^�[�����~
                stateNumber = -1;

                //���R�������~
                myRigidbody.useGravity = false;

                //�Փ˂��Ȃ���
                GetComponent<CapsuleCollider>().enabled = false;

                //�G���J�E���g�����炷
                GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>().enemycounter--;

                //�X�R�A���Z
                GameObject.Find("Canvas").GetComponent<UIController>().AddScore(900);

                //3�b��ɔj��
                Destroy(this.gameObject, 3.0f);
            }
        }
    }
}
