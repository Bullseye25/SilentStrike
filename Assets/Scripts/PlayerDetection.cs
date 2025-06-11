using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
	public bool HaveDoor = true;
	public GameObject WaveToActive;
	public GameObject DoorToActive;
	public GameObject pointLight;
	public Material materialToChange;
	public Color newColor;
	public Color TempColor;
	private AudioSource audioSource;
	public DoorController doorState;
	
	void Start()
    {
		if (materialToChange.color == newColor)
		{
			materialToChange.color = TempColor;
		}
		audioSource = GetComponent<AudioSource>();
	}

	public void ObjectToDisable()
	{
		if (GetComponent<MeshRenderer>())
		{
			GetComponent<MeshRenderer>().enabled = false;
			audioSource.enabled = false;
		}
		if(GetComponent<MeshCollider>())
        {
			GetComponent<MeshCollider>().enabled = false;
        }
		pointLight.SetActive(false);

	}

	//IEnumerator DoorClose()
 //   {
	//	yield return new WaitForSeconds(3);
	//	doorState.DoorMechanism(DoorController.DoorStates.DoorClose);
	//}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<FPSPlayer>())
		{

			materialToChange.color = newColor;
			audioSource.Play();
			pointLight.SetActive(true);

            if (GetComponent<MeshCollider>())
            {
                GetComponent<MeshCollider>().enabled = false;
            }

			WaveToActive.GetComponent<SCWave>().StartWave();

            if (HaveDoor)
            {
				doorState.DoorMechanism(DoorController.DoorStates.DoorClose);
				//StartCoroutine(DoorClose());
            }






        }
	}

}
