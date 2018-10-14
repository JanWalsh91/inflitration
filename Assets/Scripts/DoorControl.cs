using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour, IInteractable {

	public GameObject lazerDoor;
	public MeshRenderer lockScreen;
	public Material unlocked;
	public Material locked;

	public AudioSource openDoor;
	public AudioSource invalidKey;

	// Use this for initialization
	void Start () {
		if (lazerDoor) {
			lazerDoor.SetActive(true);
		}
		if (lockScreen) {
			lockScreen.material = locked;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate(Player player) {
		if (player.hasCard) {
			if (openDoor) {
				openDoor.Play();
			}
			if (lazerDoor.activeSelf) {
				lazerDoor.SetActive(false);
				lockScreen.material = unlocked;
			} else {
				lazerDoor.SetActive(true);
				lockScreen.material = locked;
			}
		} else {
			if (invalidKey) {
				invalidKey.Play();
			}
		}
	}
}
