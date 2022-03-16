using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
	
	[SerializeField] private int lifetime = 2 ;

	// Use this for initialization

void Update () {
	
		transform.Rotate(Random.Range(0, 10),Random.Range(0, 15),Random.Range (0, 22));	
        Destroy(gameObject,lifetime);
	
}
}