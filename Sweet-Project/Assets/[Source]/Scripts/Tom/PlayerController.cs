using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using RootMotion.Dynamics;
using Sirenix.OdinInspector;
using Core.Damage;

using UnityEngine.UI;

namespace Tom
{
    /// <ToDo>
    /// Get the playerRoot instead of the transform.root
    /// </ToDo>

    public class PlayerController : MonoBehaviour
    {
        #region Variables

        [NonSerialized] public int myID;
        [NonSerialized] public Transform root;

        //[Header("Managers")]
        //public SoundManager soundManager;
        //public ParticleManager particleManager;

        public Image test;

        //[Header("Health")]
        //public int lives = 3;
        //public float damage;

        [Header("Movement")]
        public float speed = 3f;
        public float tilt = 15f;
        public float jump = 40000f;

        public Transform head;

        [Header("Combat")]
        public float punchRadius;
        public Transform leftHand;
        public Transform rightHand;
        public float pickUpRadius = 3f;
        public Transform swingLocation;
        public float swingRadius;
        public float baseKnockback = 1500f;
        public int damagePerHit = 10;

        public event Action<Vector3> JumpEvent;
        public event Action<Vector3> LandEvent;

        private DamageableBehaviour myDamageableBehaviour;
        private CharacterController _controller;
        //private CharacterInputs inputs;
        private Animator _animator;

        private float _deltaTime;
        private bool _falling;
        private bool _landed;
        private bool _swinging;
        private int _jumpNumber;
        private Transform _item;
        private MuscleCollisionBroadcaster _head;
        private bool _inMenu;


        //Walter
        public struct CharacterInputs
        {
            public Rewired.Player RewiredPlayer
            {
                get; set;
            }

            public Vector2 Axis
            {
                get
                {
                    return new Vector2(RewiredPlayer.GetAxis("Move Horizontal"), RewiredPlayer.GetAxis("Move Vertical"));
                }
            }
            public Vector3 MoveInput
            {
                get
                {
                    return Vector3.ClampMagnitude(new Vector3(Axis.x, 0f, Axis.y), 1f);
                }
            }

            public float Speed
            {
                get
                {
                    return (((Axis.x < 0f) ? -Axis.x : Axis.x) + ((Axis.y < 0f) ? -Axis.y : Axis.y)) / 2f;
                }
            }

            public bool Jump
            {
                get
                {
                    return RewiredPlayer.GetButtonDown("Jump");
                }
            }

            public bool LeftPunch
            {
                get
                {
                    return RewiredPlayer.GetButtonDown("Left Punch");
                }
            }
            public bool RightPunch
            {
                get
                {
                    return RewiredPlayer.GetButtonDown("Right Punch");
                }
            }

            public bool PickUp
            {
                get
                {
                    return RewiredPlayer.GetButtonDown("Pick Up");
                }
            }
        }

        #endregion

        private void Start()
        {
            if (root == null)
            {
                root = transform.root;
            }

            myDamageableBehaviour = GetComponentInParent<DamageableBehaviour>();

            //Debug.Log(string.Format("Component In Parent = {0}", myDamageableBehaviour.name));

            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            FindObjectOfType<PauseMenu>().Menu += Menu;
            FindObjectOfType<EffectsManager>().Subscribe(myDamageableBehaviour);

            if (head != null)
            {
                _head = head.GetComponent<MuscleCollisionBroadcaster>();
                _jumpNumber = 1;
            }
        }

        private void Menu(bool menu)
        {
            _inMenu = menu;
        }

        private void Update()
        {
            _deltaTime = Time.deltaTime;
            if (!_inMenu)
            {
                if (!_falling && !_swinging)
                {
                    GetInput();
                    Movement();
                    Rotation();
                    Gravity();
                    Animations();
                }
            }
        }

        #region Get Player Input

        //Walter
        private Rewired.Player RewiredPlayer
        {
            get
            {
                return ReInput.isReady ? ReInput.players.GetPlayer(myID) : null;
            }
        }

        private CharacterInputs Inputs = new CharacterInputs();

        private void GetInput()
        {
            Inputs.RewiredPlayer = RewiredPlayer;
        }

        #endregion

        private void Movement()
        {
            if (!_swinging && !_falling)
            {
                _controller.Move(Inputs.MoveInput * speed * _deltaTime);
            }

            if (Inputs.Jump && _jumpNumber > 0)
            {
                if (_head != null)
                {
                    _jumpNumber--;
                    JumpEvent?.Invoke(transform.position);
                    _head.Hit(5, Vector3.up * jump, head.position);
                    _landed = false;
                }
            }
            if (!_falling && _controller.isGrounded)
            {
                _jumpNumber = 1;
            }
        }

        private void Rotation()
        {
            Vector3 _input = Inputs.MoveInput;

            if (_input != Vector3.zero)
            {
                transform.forward = _input;

                Vector3 _euler = transform.localEulerAngles;
                _euler.x = Mathf.Lerp(_euler.x, tilt * Inputs.Speed, 1); //Better lerp?
                transform.localEulerAngles = _euler;
            }
            else
            {
                Vector3 _euler = transform.localEulerAngles;
                _euler.x = Mathf.Lerp(_euler.x, 0f, 1); //Better lerp?
                transform.localEulerAngles = _euler;
                _animator.speed = 1f;
            }
        }

        private void Gravity()
        {
            Vector3 _velocity = _controller.velocity;

            if (!(_controller.isGrounded && _velocity.y < 0f))
            {
                _velocity.y += Physics.gravity.y;
            }

            if (_velocity.y != 0)
            {
                if (!float.IsNaN(_velocity.x * _deltaTime) && !float.IsNaN(_velocity.y * _deltaTime) && !float.IsNaN(_velocity.z * _deltaTime))
                {
                    _controller.Move(_velocity * _deltaTime);
                }
            }
        }

        private void Animations()
        {
            _animator.SetBool("Moving", Inputs.MoveInput != Vector3.zero ? true : false);

            if (Inputs.LeftPunch)
            {
                if (_item == null)
                {
                    _animator.SetTrigger("L Punch");
                }
                else
                {
                    if (!_swinging)
                    {
                        _animator.SetTrigger("Swing");
                    }
                }
            }
            if (Inputs.RightPunch)
            {
                if (_item == null)
                {
                    _animator.SetTrigger("R Punch");
                }
                else
                {
                    if (!_swinging)
                    {
                        _animator.SetTrigger("Swing");
                    }
                }
            }

            if (Inputs.PickUp && !_falling && !_swinging)
            {
                if (_item == null)
                {
                    Collider[] _colliders = Physics.OverlapSphere(transform.position, pickUpRadius);
                    List<Transform> _transforms = new List<Transform>();

                    foreach (Collider _collider in _colliders)
                    {
                        if (_collider.tag == "Pickup" && !_collider.GetComponent<PickUp>().equipped)
                        {
                            _transforms.Add(_collider.transform);
                        }
                    }

                    if (_transforms.Count > 0)
                    {
                        _item = GetClosest(_transforms);
                        _item.GetComponent<PickUp>().Equip(rightHand);
                    }
                }
                else
                {
                    _item.GetComponent<PickUp>().Unequip();
                    _item = null;
                }
            }
        }

        //public void Punch(bool isLeftHand)
        public void Punch(int _left)
        {
            Transform hand = _left == 1 ? leftHand : rightHand;
            //Transform hand = isLeftHand == true ? leftHand : rightHand;

            DebugExtension.DebugWireSphere(hand.position, punchRadius, punchRadius);

            RaycastHit[] hits = Physics.SphereCastAll(hand.position, punchRadius, hand.forward, 0f);
            //Collider[] hitColliders = Physics.OverlapSphere(hand.position, punchRadius);

            List<PlayerController> playersHit = new List<PlayerController>();

            foreach (var hit in hits)
            {
                RootFix playerRoot = hit.transform.GetComponent<RootFix>();
                if(playerRoot != null)
                {
                    PlayerController player = playerRoot.playerRoot.GetComponentInChildren<PlayerController>();
                    DamageableBehaviour damageable = playerRoot.playerRoot.GetComponent<DamageableBehaviour>();

                    var myAlignment = myDamageableBehaviour.configuration.AlignmentProvider;

                    if (damageable != null && myAlignment != null)
                    {
                        float knockback = 1f + (damageable.configuration.CurrentDamage / 100f); //EDIT

                        var hitInfo = new HitInfo
                        {
                            damageChangeInfo = new DamageChangeInfo(),
                            damagePoint = hit.point
                        };

                        bool canKnockBack = (myAlignment != damageable.configuration.AlignmentProvider);

                        if (!playersHit.Contains(player))
                        {
                            if (canKnockBack)
                            {
                                damageable.configuration.AddDamage(damagePerHit, hitInfo);
                                playersHit.Add(player);
                            }
                        }

                        MuscleCollisionBroadcaster broadcaster = hit.transform.GetComponent<MuscleCollisionBroadcaster>();

                        if (broadcaster != null && canKnockBack)
                        {
                            Vector3 heading = hit.point - hand.position;
                            print(hand.transform.forward);
                            
                            Vector3 direction = hand.transform.forward;

                            broadcaster.Hit(5, direction * (baseKnockback * (knockback * (damageable.configuration.CurrentDamage / 100f))), hit.point);
                        }
                    }
                }
            }
        }

        public void Swing(int _value)
        {
            if ((_value == 1 && _swinging) || (_value != 1 && !_swinging))
            {
                return;
            }

            _swinging = (_value == 1) ? true : false;

            if(_swinging == true)
            {
                _animator.SetLayerWeight(1, 0);
                _animator.SetLayerWeight(2, 0);
            }
            else
            {
                _animator.SetLayerWeight(1, 1);
                _animator.SetLayerWeight(2, 1);
            }
        }

        public void SwingHit()
        {
            DebugExtension.DebugWireSphere(swingLocation.position, swingRadius, 0.25f); //Delete this

            RaycastHit[] _hits = Physics.SphereCastAll(swingLocation.position, swingRadius, transform.forward, 0f);

            PickUp weapon = _item.GetComponent<PickUp>();

            List<PlayerController> playersHit = new List<PlayerController>();

            foreach (RaycastHit hit in _hits)
            {
                RootFix playerRoot = hit.transform.GetComponent<RootFix>();
                if (playerRoot != null)
                {
                    PlayerController player = playerRoot.playerRoot.GetComponentInChildren<PlayerController>();
                    DamageableBehaviour damageable = playerRoot.playerRoot.GetComponent<DamageableBehaviour>();

                    var myAlignment = myDamageableBehaviour.configuration.AlignmentProvider;

                    if (damageable != null && myAlignment != null)
                    {
                        float knockback = 1f + (damageable.configuration.CurrentDamage / 100f); //EDIT

                        var hitInfo = new HitInfo
                        {
                            damageChangeInfo = new DamageChangeInfo(),
                            damagePoint = hit.point
                        };

                        bool canKnockBack = (myAlignment != damageable.configuration.AlignmentProvider);

                        //print("Obj: " + player.name + ", Count: " + playersHit.Count + ", Contains: " + playersHit.Contains(player));
                        if (!playersHit.Contains(player))
                        {
                            if (canKnockBack)
                            {
                                damageable.configuration.AddDamage((weapon != null ? weapon.damage : damagePerHit), hitInfo);
                                playersHit.Add(player);
                            }
                        }
                        MuscleCollisionBroadcaster broadcaster = hit.transform.GetComponent<MuscleCollisionBroadcaster>();

                        if (broadcaster != null && canKnockBack)
                        {
                            broadcaster.Hit(5, rightHand.transform.forward * (baseKnockback * (knockback * (damageable.configuration.CurrentDamage / 100f))), hit.point);
                        }
                    }
                }
                /*
                if (_hit.transform.root != transform.root)
                {
                    Debug.Log("RUGHAARINMIJNRIETJE TWEE");

                    //PlayerController _player = null;
                    DamageableBehaviour _damageable = null;
                    IAlignmentProvider myAlignment = null;

                    if (_hit.transform.root.tag == "Player")
                    {
                        //_player = _hit.transform.root.GetComponentInChildren<PlayerController>();
                        _damageable = _hit.transform.root.GetComponentInChildren<DamageableBehaviour>();
                    }

                    float knockback = 1f;
                    if (_damageable != null)
                    {
                        myAlignment = myDamageableBehaviour.configuration.AlignmentProvider;

                        int weaponDamagePerHit = ((_item != null) ? _item.GetComponent<PickUp>().damage : damagePerHit);

                        _damageable.configuration.CheckDamage(weaponDamagePerHit, myAlignment, swingLocation.position);
                    }

                    MuscleCollisionBroadcaster broadcaster = _hit.transform.GetComponent<MuscleCollisionBroadcaster>();

                    if (broadcaster != null && _damageable.CanKnockback(myAlignment))
                    {
                        Vector3 _heading = _hit.point - swingLocation.position;
                        float _distance = _heading.magnitude;
                        Vector3 _direction = _heading / _distance;

                        broadcaster.Hit(5, _direction * (baseKnockback * knockback), _hit.point); //Edit this
                    }
                }
                */
            }
        }

        public void Fall(int _fall)
        {
            _falling = (_fall == 1) ? true : false;
            if (_falling)
            {
                if (_item != null)
                {
                    _item.GetComponent<PickUp>().Unequip();
                    _item = null;
                }
            }
        }

        private Transform GetClosest(List<Transform> _transforms)
        {
            if (_transforms.Count == 1)
            {
                return _transforms[0];
            }
            else
            {
                KeyValuePair<Transform, float> _closest = new KeyValuePair<Transform, float>();

                for (int i = 0; i < _transforms.Count; i++)
                {
                    Vector3 _offset = _transforms[i].position - transform.position;
                    float _distance = _offset.sqrMagnitude;

                    if (i == 0 || _distance < _closest.Value)
                    {
                        _closest = new KeyValuePair<Transform, float>(_transforms[i], _distance);
                    }
                }

                return _closest.Key;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit _hit)
        {
            if (_falling && !_landed)
            {
                LandEvent?.Invoke(transform.position);
                _landed = true;
            }

        }
    }

}