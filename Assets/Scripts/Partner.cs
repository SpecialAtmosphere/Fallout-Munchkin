using UnityEngine;
using System.Collections;

public class Partner : MonoBehaviour
{
	public int Bonus;

	public void Activate(CardPlayer player)
	{
		if (player.CanTakePartner)
		{
			var card = this.GetComponentInParent<Card>();
			player.Partners.Add(card);
			player.Hand.Remove(card);
			var inv = player.transform.FindChild("Inventory");
			card.transform.parent = inv.transform;
			card.transform.position = new Vector3(28, 15, 0);
			card.transform.localScale = new Vector3(1, 1, 1);
			card.SwitchGameLocation(GameLocations.InUse);
		}
	}
}
