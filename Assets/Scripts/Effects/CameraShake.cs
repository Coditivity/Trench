using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Transform _childTransform = null;
    Quaternion _initialChildRotation = Quaternion.identity;
	// Use this for initialization
	void Start () {
        _initialChildRotation = _childTransform.localRotation;
        _prevChildRotation = _childTransform.localRotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Vector3 _prevChildRotation = Vector3.zero;
    Vector3 _rotationDelta = Vector3.zero;
    void LateUpdate()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        _rotationDelta = (_childTransform.localRotation.eulerAngles - _initialChildRotation.eulerAngles) ;
        transform.rotation = Quaternion.Euler(currentRotation + _rotationDelta); 
        _prevChildRotation = _childTransform.localRotation.eulerAngles;
        _childTransform.localRotation = _initialChildRotation;
    }
}
