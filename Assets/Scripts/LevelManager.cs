using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
	public GameObject platformPrefab;
	public List<Platform> platformPool;
	public PlayerController player;
	public List<List<Platform>> platforms;
	public ArenaChecker arena;
	public Text scoreCounter;
	public Button restartButton;
	
	public int lanes;
	public float lineGap;
	public float paddingFromCamera;
	public float laneWidth;
	public float score;

	private float nextYPos;
	float lastPos;
	private Vector2 playerOriginPos;
	private Vector3 cameraOriginPos;

	private void Awake() {
		platformPool = new List<Platform>();
		platforms = new List<List<Platform>>();
		nextYPos = 0;
		lastPos = 0;
		score = 0;
		playerOriginPos = player.transform.position;
		cameraOriginPos = gameObject.transform.position;
		restartButton.onClick.AddListener(resetGame);
	}

	private void Update() {
		laneWidth = arena.arenaWidth / lanes;
		while (nextYPos >= paddingFromCamera + arena.transform.position.y) {
			var temp = new List<Platform>();
			for (int i = 0; i < lanes; i++) {
				var go = getNextAvailablePlatform();
				go.Spawn(Random.Range(0f, 1f) >= 0.8f, new Vector2(laneWidth/2 + arena.leftBorder + laneWidth * i, nextYPos));
				temp.Add(go);
			}
			platforms.Add(temp);
			nextYPos += lineGap;
			temp[Random.Range(0, temp.Count)].setAsPass();
			if (lastPos > arena.gameObject.transform.position.y) {
				score += lastPos - arena.gameObject.transform.position.y;
				lastPos = arena.gameObject.transform.position.y;
				scoreCounter.text = Mathf.Floor(score).ToString();
			}
		}
		
		foreach (var platform in platformPool) {
			if(platform.gameObject.activeSelf)platform.DoUpdate();
		}
	}

	public Platform getNextAvailablePlatform() {
		foreach (var platform in platformPool) {
			if (!platform.gameObject.activeSelf) return platform;
		}
		var go = Instantiate(platformPrefab);
		var newPlatform = go.GetComponent<Platform>();
		newPlatform.Init(arena);
		platformPool.Add(newPlatform);
		return newPlatform;
	}

	public void triggerPassPlatform() {
		foreach (var platform in platforms[0]) {
			platform.TriggerDeath();
		}
		platforms.RemoveAt(0);
	}

	public void TriggerDeath() {
		restartButton.gameObject.SetActive(true);
	}

	public void resetGame() {
		restartButton.gameObject.SetActive(false);
		nextYPos = 0;
		lastPos = 0;
		score = 0;
		scoreCounter.text = Mathf.Floor(score).ToString();
		player.gameObject.transform.position = playerOriginPos;
		gameObject.transform.position = cameraOriginPos;
		foreach (var platform in platformPool) {
			platform.gameObject.SetActive(false);
		}
		platforms = new List<List<Platform>>();
		player.gameObject.SetActive(true);
	}

}
