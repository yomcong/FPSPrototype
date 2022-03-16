using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {
	
[SerializeField] private float amount = 0.02f;
[SerializeField] private float maxAmount = 0.03f;
[SerializeField] private float Smooth = 3;
[SerializeField] private float SmoothRotation = 2;
[SerializeField] private float tiltAngle = 2;

private Vector3 def; 

	// Use this for initialization
	void Start () {
	def = transform.localPosition;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	  float factorX = -Input.GetAxis("Mouse X") * amount;
      float factorY = -Input.GetAxis("Mouse Y") * amount;
       
        if (factorX > maxAmount)
            factorX = maxAmount;
       
        if (factorX < -maxAmount)
                factorX = -maxAmount;
 
        if (factorY > maxAmount)
                factorY = maxAmount;
       
        if (factorY < -maxAmount)
                factorY = -maxAmount;
               
 
        Vector3 Final = new Vector3(def.x+factorX, def.y+factorY, def.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, Final, Time.deltaTime * Smooth);
       
             
        float tiltAroundZ = Input.GetAxis("Mouse X") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Mouse Y") * tiltAngle;
        Quaternion target = Quaternion.Euler (tiltAroundX, 0, tiltAroundZ);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target,Time.deltaTime * SmoothRotation);    
		
	}
}
