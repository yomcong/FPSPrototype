using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour {
	
	[SerializeField] private int lifetime = 5 ;

void Update () {
	
        Destroy(gameObject,lifetime);
	
}
}