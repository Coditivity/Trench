using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class SpriteScript : MonoBehaviour {

	//Camera Transform
    [FormerlySerializedAs("camera")]
	public Transform mainCamera;
	public Sprite 	spriteBack,
					sprite1,sprite2,
					sprite3,spriteFront,
					sprite5,sprite6,sprite7;


	//Sprite parent
	private Transform parent;
	
	Vector3 tempPos;
	Vector3 vectorBetween;
	Vector3 vectorB;
	float enemyAngle;
    
    private Animator _animator = null;
    [SerializeField]
    private string _parameterNameFrameBlend = "FrameBlend";

    private int _parameterHashFrameBlend = -1;

	void Awake () {
		parent = GetComponentInParent<Transform>();
        

	}

    void Start()
    {
        _animator = GetComponent<Animator>();
        _parameterHashFrameBlend = Animator.StringToHash(_parameterNameFrameBlend);
    }


	void Update () {
		//Look at
		Vector3 v = mainCamera.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt(mainCamera.transform.position - v);

		//Rotation
		vectorBetween =  (parent.position - mainCamera.position);
		vectorB = (transform.parent.forward);
	
		changeSpriteCheck(SignedAngleBetween(vectorBetween,vectorB,Vector3.up));


	}

    private void SetFrame(float frameNumber)
    {
        float blend = (1f / 7f) * frameNumber;
        if(blend > 1 || blend < 0)
        {
            Debug.LogError("Invalid blend value. Should be in the 0 to 1 range");
        }
        _animator.SetFloat(_parameterHashFrameBlend, blend);
    }

   

	float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n){
		// angle in [0,180]
		float angle = Vector3.Angle(a,b);
		float sign = Mathf.Sign(Vector3.Dot(n,Vector3.Cross(a,b)));
		
		// angle in [-179,180]
		float signed_angle = angle * sign;
		
		//angle in [0,360] (not used but included here for completeness)
		float angle360 =  (signed_angle + 180) % 360;
		
		return angle360;
	}

		void changeSpriteCheck(float enemyAngle){
        GameLogger.ClearLog();
		if (enemyAngle >= 157.5 && enemyAngle < 202.5)  {
            //GetComponent<SpriteRenderer> ().sprite = spriteBack as Sprite;
            SetFrame(0);
        }
		//moturs rotation 1
		else if ( enemyAngle >= 202.5  && enemyAngle < 225 ) {
            //GetComponent<SpriteRenderer> ().sprite = sprite1 as Sprite;
            SetFrame(1);
        }
		//moturs rotation 2 (sedd från sidan)
		else if ( enemyAngle >= 225 && enemyAngle < 270 ) {
            //GetComponent<SpriteRenderer> ().sprite = sprite2 as Sprite;
            SetFrame(2);
        }
		//moturs rotation 3
		else if ( enemyAngle >= 270 && enemyAngle < 315 ) {
            //GetComponent<SpriteRenderer> ().sprite = sprite3 as Sprite;
            SetFrame(3);
        }
		//back
		else if ( enemyAngle >= 315 || enemyAngle < 22.5 ) {
            //GetComponent<SpriteRenderer> ().sprite = spriteFront as Sprite;
            SetFrame(4);
		}
		//moturs 4
		else if ( enemyAngle >= 22.5 && enemyAngle < 67.5 ) {
            //GetComponent<SpriteRenderer> ().sprite = sprite5 as Sprite;
            SetFrame(5);
        }
		//moturs 5 (sedd från sidan)
		else if ( enemyAngle >= 67.5 && enemyAngle < 112.5 ) {
            SetFrame(6);
        }
		//moturs 6
		else if ( enemyAngle >= 112.5  && enemyAngle < 157.5 ) {
            SetFrame(7);
        }
	}
}

