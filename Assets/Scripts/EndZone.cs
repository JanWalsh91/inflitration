using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour, IInteractable {

	public void Activate(Player player) {
		//GameManager.instance.UpdateOnScreenMessage("You win!");
		//Invoke("Reset", 3);
		Reset();
	}

	void Reset() {
		GameManager.instance.RestartSimulation(success: true);
	}
}
