using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehaviour : MonoBehaviour {

    
    Animator _animator = null;

    [SerializeField]
    private string _animParamFlinch = "Flinch";
    [SerializeField]
    private string _animParamOverDie = "OverDie";
    [SerializeField]
    private string _animParamDie = "Die";


    private Collider _collider = null;
    private AudioSource _audioSource = null;

    private int _animParamHashFlinch = -1;
    private int _animParamHashOverDie = -1;
    private int _animParamHashDie = -1;

    int _currentHealth = 0;
    DemonData _dd = null;
	// Use this for initialization
	void Start () {
        _dd = DemonData.Instance;
        _animator = GetComponent<Animator>();
        PlayerAttack.Instance.OnPrimaryAttackHitListener.AddListener(OnPrimaryAttackHitCallBack);
        PlayerAttack.Instance.OnPrimaryAttackEndListener.AddListener(OnPrimaryAttackEndCallBack);
        _animParamHashFlinch = Animator.StringToHash(_animParamFlinch);
        _animParamHashOverDie = Animator.StringToHash(_animParamOverDie);
        _animParamHashDie = Animator.StringToHash(_animParamDie);
        _currentHealth = _dd.maxHealth;
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnPrimaryAttackHitCallBack(AttackHitData attackHitData)
    {
        if (_currentHealth <= 0)
        {
         //   return;
        }
        if (attackHitData.raycastHit.collider.gameObject == gameObject) {
           
            _currentHealth -= attackHitData.damage;
            if (attackHitData.bLastProjectile) {
                if (_currentHealth <= _dd.overkillHealth)
                {
                    _animator.SetTrigger(_animParamHashOverDie);
                    OnDeath(true);

                }
                else if (_currentHealth <= 0)
                {
                    _animator.SetTrigger(_animParamHashDie);
                    OnDeath(false);
                }
                else {
                    _animator.SetTrigger(_animParamHashFlinch);
                    _audioSource.PlayOneShot(_dd.audioClipHit);
                }
            }
                                    
           
        }       

    }


    void OnDeath(bool overDeath)
    {
        _collider.enabled = false;        
        _audioSource.PlayOneShot(_dd.audioClipDeath);
        
    }
    private void OnPrimaryAttackEndCallBack()
    {
        

    }

    // Update is called once per frame
    void Update () {
		
	}
}
