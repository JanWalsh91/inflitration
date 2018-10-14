using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour {

	public GameObject joint;
	public float angle1;
	public float angle2;
	public float speed;

	float x, z;

	// Use this for initialization
	void Start () {
		x = joint.transform.rotation.x;
		z = joint.transform.rotation.z;
	}
	
	// Update is called once per frame
	void Update () {
		Rotate();
	}

	void Rotate() {
		float angle = angle1 + ((Mathf.Cos(Time.time * speed) + 1) / 2) * angle2;
		joint.transform.rotation = Quaternion.Euler(x, angle, z);
	}
}
