using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour, IInteractable  {

	bool isOn = false;
	bool showedHint = false;

	public ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		isOn = false;
		showedHint = false;
	}

	public void Activate(Player player) {
		if (!particleSystem) { Debug.Log("No aprticle system!"); return; }
		if (!isOn) {
			particleSystem.Play();
			player.inSmoke = true;
			if (!showedHint) {
				GameManager.instance.UpdateOnScreenMessage("Smoke! Maybe that will help me hide from the cameras ...");
				showedHint = true;
			}
		} else {
			particleSystem.Stop();
			player.inSmoke = false;
		}
		isOn = !isOn;
	}

}
