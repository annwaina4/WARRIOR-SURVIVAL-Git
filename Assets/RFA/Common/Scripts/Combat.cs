using UnityEngine;
using NaughtyCharacter;

namespace Retro.ThirdPersonCharacter
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    public class Combat : MonoBehaviour
    {
        private const string attackTriggerName = "Attack";
        private const string specialAttackTriggerName = "Ability";

        private Animator _animator;
        private PlayerInput _playerInput;

        public bool AttackInProgress {get; private set;} = false;

        public GameObject fireballPrefab;

        public GameObject meteorPrefab;

        private GameObject childObj;

        //private float speed = 300.0f;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
            childObj = transform.GetChild(1).gameObject;

        }

        private void Update()
        {
            if(_playerInput.AttackInput && !AttackInProgress)
            {
                Attack();
                //ファイアーボールの生成
                //GameObject fireball = (GameObject)Instantiate(fireballPrefab,childObj.transform.position,this.transform.rotation);
                //Rigidbody fireballRigidbody = fireball.GetComponent<Rigidbody>();
                //fireballRigidbody.AddForce(transform.forward *this.speed);
            }
            else if (_playerInput.SpecialAttackInput && !AttackInProgress)
            {
                SpecialAttack();
                //メテオの生成
                GameObject Meteor = (GameObject)Instantiate(meteorPrefab, childObj.transform.position, this.transform.rotation);
            }
        }

        private void SetAttackStart()
        {
            AttackInProgress = true;             
        }

        private void SetAttackEnd()
        {
            AttackInProgress = false;
        }

        private void Attack()
        {
            _animator.SetTrigger(attackTriggerName);
        }

        private void SpecialAttack()
        {
            _animator.SetTrigger(specialAttackTriggerName);
        }

        private void AttackEvent()
        {
            //ファイアーボールの生成
            GameObject fireball = (GameObject)Instantiate(fireballPrefab, childObj.transform.position, this.transform.rotation);
        }
    }
}