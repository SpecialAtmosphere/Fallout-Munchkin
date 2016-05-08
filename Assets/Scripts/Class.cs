using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Class : MonoBehaviour
{
	public void Activate(CardPlayer player)
	{
		var card = this.GetComponentInParent<Card>();
		//Add to Class Array
		player.Classes.Add(card);
		//Remove from Hand List
		player.Hand.Remove(card);
		//transform
		var inv = player.transform.FindChild("Inventory");
		card.transform.parent = inv.transform;
		card.transform.position = new Vector3(14, 15, 0);
		card.transform.localScale = new Vector3(1, 1, 1);
		card.Selected = false;
		GameObject.FindObjectOfType<Game>().CurrentCard = null;
	}
}
