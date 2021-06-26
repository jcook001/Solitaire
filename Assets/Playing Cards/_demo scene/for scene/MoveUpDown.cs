using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour {

	public new GameObject camera;
	public GameObject img;
	// Use this for initialization
	void Start () {
		camera = (GameObject)this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.DownArrow) && camera.transform.position.y >= -47) {
			camera.transform.position -= camera.transform.up * 12 * Time.deltaTime;
			Destroy (img);

		}
		if (Input.GetKey (KeyCode.UpArrow) && camera.transform.position.y <= 9) {
			camera.transform.position += camera.transform.up * 10 * Time.deltaTime;
		}
			}
}
