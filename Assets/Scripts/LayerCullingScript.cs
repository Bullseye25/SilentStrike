using UnityEngine;
using System.Collections;

public class LayerCullingScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Camera camera = GetComponent<Camera>();
		float[] distances = new float[32];
		distances[14] = 180;//Trees
		distances[16] = 100;//Trees
		distances[21] = 80;//Trees

		camera.layerCullDistances = distances;
	}

}
