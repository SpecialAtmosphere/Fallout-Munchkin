using UnityEngine;

public class OnJoinedInstantiate : MonoBehaviour
{
	public GameObject[] PrefabsToInstantiate;
	public void OnJoinedRoom()
	{
		if (this.PrefabsToInstantiate != null)
		{
			foreach (GameObject o in this.PrefabsToInstantiate)
			{
				//Debug.Log("Instantiating: " + o.name);
				PhotonNetwork.Instantiate(o.name, new Vector3(), Quaternion.identity, 0);
			}
		}
	}
}
