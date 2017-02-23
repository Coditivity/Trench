using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class AnimatorStates : MonoBehaviour {

    /// <summary>
    /// Enum holding different parameters used in animators
    /// </summary>
    public enum AnimationParameter 
    {
        Fire=0,
        Unwield=1,
        FireStop=2,
        CameraShakePistol=3,
        CameraShakeShotGun=4,
        CameraShakeMachineGun=5

    }

    
    private static AnimatorStates s_instance = null;
    [FormerlySerializedAs("_animators")]
    [SerializeField]
    private Animator[] _weaponAnimators = null;
    

    [SerializeField]
    private AnimationParameterInfo[] animatorParameters = null;


    public static void Set(AnimationParameter parameter, Animator animator)
    {
        s_instance.animatorParameters[(int)parameter].Set(animator);
    }

    public static void UnSet(AnimationParameter parameter, Animator animator)
    {
        s_instance.animatorParameters[(int)parameter].UnSet(animator);
    }

    public static void ResetTrigger(AnimationParameter parameter, Animator animator)
    {
        s_instance.animatorParameters[(int)parameter].ResetTrigger(animator);
    }
    public static void Set(AnimationParameter parameter, WeaponBase.WeaponType weaponType)
    {
        s_instance.animatorParameters[(int)parameter].Set(s_instance._weaponAnimators[(int)weaponType-1]);

    }

    public static void UnSet(AnimationParameter parameter, WeaponBase.WeaponType weaponType)
    {
        s_instance.animatorParameters[(int)parameter].UnSet(s_instance._weaponAnimators[(int)weaponType-1]);
    }

    public static void ResetTrigger(AnimationParameter parameter, WeaponBase.WeaponType weaponType)
    {
        s_instance.animatorParameters[(int)parameter].ResetTrigger(s_instance._weaponAnimators[(int)weaponType-1]);
    }

    

    public static bool GetBool(AnimationParameter parameter, WeaponBase.WeaponType weaponType)
    {
        return s_instance.animatorParameters[(int)parameter].GetBool(s_instance._weaponAnimators[(int)weaponType]);
    }

    public static bool Compare(Animator animator, int layerIndex, AnimationParameter parameter1
        , AnimationParameter parameter2)
    {
        return s_instance.animatorParameters[(int)parameter1]
            .Compare(animator, layerIndex, s_instance.animatorParameters[(int)parameter2].NameHash);
    }

    // Use this for initialization
    void Start() {
        s_instance = this;
       
        foreach (AnimationParameterInfo ap in animatorParameters)
        {
            ap.CalculateHash();
        }
    }

    // Update is called once per frame
    void Update() {

    }

    void OnDestroy()
    {
        s_instance = null;
    }


    [System.Serializable]
    public enum AnimParameterType { Bool, Trigger }

    [System.Serializable]
    public class AnimationParameterInfo
    {
        public string parameterName;
        public AnimParameterType parameterType;
        private int _parameterHash;
        public int NameHash {
            get {
                return _parameterHash;
            }
        }
        public AnimationParameterInfo()
        {
            _parameterHash = Animator.StringToHash(parameterName);
        }
        public void CalculateHash()
        {
            _parameterHash = Animator.StringToHash(parameterName);
        }

        public void Set(Animator animator)
        {
            if(parameterType == AnimParameterType.Bool)
            {
                animator.SetBool(_parameterHash, true);
            }
            else if (parameterType == AnimParameterType.Trigger)
            {
                animator.SetTrigger(_parameterHash);
            }
        }

        public void UnSet(Animator animator)
        {
            if (parameterType == AnimParameterType.Bool)
            {
                animator.SetBool(_parameterHash, false);
            }
            else if(parameterType == AnimParameterType.Trigger)
            {
                Debug.LogError("Cannot unset the trigger named>>" + parameterName);
            } 
        }

        public void ResetTrigger(Animator animator)
        {
            if (parameterType == AnimParameterType.Bool)
            {
                Debug.LogError("Cannot untrigger the bool named>>" + parameterName);                
            }
            else if (parameterType == AnimParameterType.Trigger)
            {
                animator.ResetTrigger(_parameterHash);
            }
        }

        public bool GetBool(Animator animator)
        {
            if(parameterType == AnimParameterType.Bool)
            {
                return animator.GetBool(_parameterHash);
            }
            else
            {
                Debug.LogError("Parameter is not a bool to getBool");
                return false;
                
            }
        }

        public bool Compare(Animator animator,int layerIndex, int hashToCompare)
        {
            return animator.GetAnimatorTransitionInfo(layerIndex).nameHash == _parameterHash;
        }
        

    }
}
