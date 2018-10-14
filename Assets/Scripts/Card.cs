using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour, IInteractable {

	public GameObject card;

	public AudioSource cardPickUp;

	void Start() {
		cardPickUp = GetComponent<AudioSource>();
		if (card != null) {
			card.SetActive(true);
		}
	}

	public void Activate(Player player) {
		player.hasCard = true;
		if (card != null) {
			card.SetActive(false);
			if (cardPickUp) {
				cardPickUp.Play();
				GameManager.instance.UpdateOnScreenMessage("A Card Key. Maybe this can unlock something.");
			}
		}
	}
}
