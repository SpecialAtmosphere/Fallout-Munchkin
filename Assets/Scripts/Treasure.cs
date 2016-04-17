using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour
{
	public string Name;
	public string Discription;
	public TreasureType Type;
	public bool AddLvl;

	public enum TreasureType
	{
		Staff,
		Lvl,
		Other
	}

	public void Use(CardPlayer player)
	{
		//TODO: Дописать подробности и описание
		var card = this.GetComponentInParent<Card>();
		switch (this.Type)
		{
			case TreasureType.Lvl:
				player.Level++;
				player.Hand.Remove(card);
				GameObject.FindObjectOfType<Game>().tReset(card);
				break;
			case TreasureType.Other:
				break;
		}
	}
}
