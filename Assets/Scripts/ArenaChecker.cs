using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaChecker : MonoBehaviour {
	public Camera mainCamera;
	public GameObject player;
	public float playerCameraOffset;
	
	[HideInInspector]
	public float leftBorder;
	[HideInInspector]
	public float rightBorder;

	public float arenaWidth {
		get { return rightBorder - leftBorder; }
	}

	private Plane[] fustrum;

	
	private void Awake() {
		fustrum = new Plane[6];
		leftBorder = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
		rightBorder = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
	}
	
	private void Update() {
		fustrum = GeometryUtility.CalculateFrustumPlanes(mainCamera);
		if (player.transform.position.y <= mainCamera.gameObject.transform.position.y + playerCameraOffset) {
			var pos = mainCamera.gameObject.transform.position;
			pos.y = player.transform.position.y - playerCameraOffset;
			mainCamera.gameObject.transform.position = pos;
		}
		
	}

	public bool CheckIfInView(Bounds bounds) {
		return GeometryUtility.TestPlanesAABB(fustrum, bounds);
	}
}
