using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public Player player;
	public float dangerLevel = 0.0f;
	float dangerLevelIncrementRun = 0.1f;
	float dangerLevelIncrementLight = 0.1f;
	float dangerLevelIncrementCamera = 0.4f;
	float dangerLevelSmokeBuff = 0.3f;

	public Slider sliderDangerLevel;
	public Image sliderFill;

	bool alarmsOn = false;

	public AudioSource normalMusic;
	public AudioSource panicMusic;
	public AudioSource[] AlarmsMusic;
	public AudioSource restartSimulationMusic;

	public Text onScreenMessage;
	bool fadeOutMessage = false;
	float alphaLevel = 0f;
	float timeSinceFadeOutStart = 0f;
	Coroutine coroutineHideScreenMessage = null;
	Coroutine coroutineRestart = null;

	bool restarting = false;

	public static GameManager instance = null;
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		//DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		alarmsOn = false;
		fadeOutMessage = false;
		alphaLevel = 0f;
		timeSinceFadeOutStart = 0f;
		restarting = false;
		UpdateOnScreenMessage("Stealth mission: aquire secret documents.");
	}
	
	// Update is called once per frame
	void Update () {
		if (restarting) return;
		UpdateDangerLevel();
		UpdateFadeOutMessage();
	}

	void UpdateDangerLevel() {
		if (!player) { Debug.Log("no player!"); return; }

		if (player.isRunning) {
			dangerLevel += dangerLevelIncrementRun * Time.deltaTime;
		}

		if (player.inLight) {
			dangerLevel += dangerLevelIncrementLight * Time.deltaTime;
		}

		if (player.inCameraView) {
			if (player.inSmoke) {
				dangerLevel += (dangerLevelIncrementCamera - dangerLevelSmokeBuff) * Time.deltaTime;
			} else {
				dangerLevel += (dangerLevelIncrementCamera) * Time.deltaTime;
			}
		}

		if (!player.inCameraView && !player.isRunning && !player.inLight && dangerLevel > 0) {
			dangerLevel -= dangerLevelIncrementCamera * Time.deltaTime;
		}

		UpdateDangerLevelGUI();
		if (dangerLevel > 0.99f && !restarting) {
			RestartSimulation(success: false);
		}
	}

	void UpdateDangerLevelGUI() {
		if (!sliderDangerLevel) { Debug.Log("no slider!"); return; }

		dangerLevel = Mathf.Clamp(0, dangerLevel, 1);
		sliderDangerLevel.value = dangerLevel;
		if (dangerLevel > 0.75f) {
			ToggleAlarms(true);
			sliderFill.color = Color.red;
		} else {
			ToggleAlarms(false);
			sliderFill.color = Color.grey;
		}
	}

	void ToggleAlarms(bool toggleOn) {
		if (toggleOn == alarmsOn) return;

		if (toggleOn) {
			alarmsOn = true;
			Debug.Log("toggle panic music");
			normalMusic.Stop();
			panicMusic.Play();
			foreach (AudioSource audioSource in AlarmsMusic) {
				audioSource.Play();
			}
		} else {
			alarmsOn = false;
			normalMusic.Play();
			Debug.Log("toggle normal music");
			panicMusic.Stop();
			foreach (AudioSource audioSource in AlarmsMusic) {
				audioSource.Stop();
			}
		}
	}

	public void UpdateOnScreenMessage(string message) {
		//Debug.Log("UpdateOnScreenMessage to " + message);
		alphaLevel = 1f;
		onScreenMessage.gameObject.SetActive(true);
		onScreenMessage.gameObject.GetComponent<CanvasRenderer>().SetAlpha(alphaLevel);
		onScreenMessage.text = message;
		if (coroutineHideScreenMessage != null) {
			StopCoroutine(coroutineHideScreenMessage);
		}
		coroutineHideScreenMessage = StartCoroutine(HideOnScreenMessage());
	}

	IEnumerator HideOnScreenMessage() {
		//Debug.Log("HideOnScreenMessage");
		yield return new WaitForSeconds(2f);
		timeSinceFadeOutStart = Time.time;
		fadeOutMessage = true;
	}

	void UpdateFadeOutMessage() {
		if (!fadeOutMessage) return;
		alphaLevel -= Time.deltaTime;
		onScreenMessage.gameObject.GetComponent<CanvasRenderer>().SetAlpha(alphaLevel);
		if (alphaLevel < 0.0f) {
			alphaLevel = 1f;
			fadeOutMessage = false;
			onScreenMessage.gameObject.SetActive(false);
		}
	}

	public void RestartSimulation(bool success) {
		restarting = true;
		if (success) {
			UpdateOnScreenMessage("Simulation success");
		} else {
			UpdateOnScreenMessage("Simulation fail. Restarting simulation ...");
		}
		if (coroutineRestart != null) {
			return;
		}
		restartSimulationMusic.Play();
		coroutineRestart = StartCoroutine(Reset());
	}

	public IEnumerator Reset() {
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
