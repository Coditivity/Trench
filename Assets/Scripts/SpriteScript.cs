using UnityEngine;
using System.Collections;

public class SpriteScript : MonoBehaviour {

	//Camera Transform
	public Transform camera;
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


	void Awake () {
		parent = GetComponentInParent<Transform>();

	}


	void Update () {
		//Look at
		Vector3 v = camera.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt(camera.transform.position - v);

		//Rotation
		vectorBetween =  (parent.position - camera.position);
		vectorB = (transform.parent.forward);
	
		changeSpriteCheck(SignedAngleBetween(vectorBetween,vectorB,Vector3.up));


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
		if (enemyAngle > 157.5 && enemyAngle < 202.5)  {
			GetComponent<SpriteRenderer> ().sprite = spriteBack as Sprite;
		}
		//moturs rotation 1
		else if ( enemyAngle >202.5  && enemyAngle < 225 ) {
			GetComponent<SpriteRenderer> ().sprite = sprite1 as Sprite;
		}
		//moturs rotation 2 (sedd från sidan)
		else if ( enemyAngle > 225 && enemyAngle < 270 ) {
			GetComponent<SpriteRenderer> ().sprite = sprite2 as Sprite;
		}
		//moturs rotation 3
		else if ( enemyAngle > 270 && enemyAngle < 315 ) {
			GetComponent<SpriteRenderer> ().sprite = sprite3 as Sprite;
		}
		//back
		else if ( enemyAngle > 315 || enemyAngle < 22.5 ) {
			GetComponent<SpriteRenderer> ().sprite = spriteFront as Sprite;
		}
		//moturs 4
		else if ( enemyAngle > 22.5 && enemyAngle < 67.5 ) {
			GetComponent<SpriteRenderer> ().sprite = sprite5 as Sprite;
		}
		//moturs 5 (sedd från sidan)
		else if ( enemyAngle > 67.5 && enemyAngle < 112.5 ) {
			GetComponent<SpriteRenderer> ().sprite = sprite6 as Sprite;
		}
		//moturs 6
		else if ( enemyAngle > 112.5  && enemyAngle < 157.5 ) {
			GetComponent<SpriteRenderer> ().sprite = sprite7 as Sprite;
		}
	}
}

