using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[HideInInspector]
	public bool inCameraView = false;
	[HideInInspector]
	public bool isRunning = false;
	[HideInInspector]
	public bool inLight = false;
	//[HideInInspector]
	public bool inSmoke = false;
	//[HideInInspector]
	public bool hasCard = false;

	public IInteractable interactObject = null;

	Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		inCameraView = false;
		isRunning = false;
		inLight = false;
		inSmoke = false;
		hasCard = false;
		interactObject = null;
	}

	// Update is called once per frame
	void Update() {
		if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))) {
			isRunning = true;
		} else {
			isRunning = false;
		}
		if (interactObject != null && Input.GetKeyDown(KeyCode.E)) {
			interactObject.Activate(this);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Camera")) {
			inCameraView = true;
		} else if (other.gameObject.CompareTag("Light")) {
			inLight = true;
		} else if (other.gameObject.CompareTag("Interactible")) {
			Debug.Log("OnTriggerEnter interactible");
			interactObject = other.gameObject.GetComponent<IInteractable>();
			GameManager.instance.UpdateOnScreenMessage("Press E to interact");
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Camera")) {
			inCameraView = false;
		} else if (other.gameObject.CompareTag("Light")) {
			inLight = false;
		} else if (other.gameObject.CompareTag("Interactible")) {
			Debug.Log("OnTriggerExit interactible");
			interactObject = null;
			//GameManager.instance.UpdateOnScreenMessage("");
		}
	}
}
