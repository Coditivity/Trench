using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponSwayAnimatorScript : MonoBehaviour {

    [SerializeField]
    Animator _weaponSwayAnimator = null;
    [SerializeField]
    FirstPersonController _firstPersonController = null;
    [SerializeField]
    [Tooltip("Lower the step value, higher the smoothness. Shouldn't be zero.")]
    float _stepValue = 1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public static void MoveValue(ref float curVal, float toVal, float step)
    {
        if (toVal < curVal)
        {
            curVal -= Mathf.Abs(step);
            if (curVal < toVal)
            {
                curVal = toVal;
            }
        }
        else
        {
            curVal += Mathf.Abs(step);
            if (curVal > toVal)
            {
                curVal = toVal;
            }
        }
        


    }


    [SerializeField]
    string _movementSpeedBlendParameterName = "movementSpeed";
    float _smoothenedMoveSpeed = 0;
    void LateUpdate()
    {


        MoveValue(ref _smoothenedMoveSpeed, _firstPersonController.GetInstantaneousVelocity().magnitude, _stepValue * Time.deltaTime);
        _weaponSwayAnimator.SetFloat(_movementSpeedBlendParameterName
            , _smoothenedMoveSpeed / _firstPersonController.MaxMovementSpeed);
    }
}
