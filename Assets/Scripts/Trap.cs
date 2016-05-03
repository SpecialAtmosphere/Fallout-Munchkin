using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Trap : MonoBehaviour
{
	public void Activate(CardPlayer player)
	{
		var card = this.GetComponentInParent<Card>();
		var door = card.GetComponent<Door>();
		var game = GameObject.FindObjectOfType<Game>();
		switch (door.Name)
		{
			case "Poisoning":
				player.Level--;
				ToReset(card);
				break;
			case "Overloading":
				var hand =
					from c in player.Hand
					where c.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff
					&& c.GetComponent<Treasure>().GetComponent<Staff>().StuffType != StuffTypes.Junk
					select c;
				var inventary = from c in player.Inventary
								where c.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff
								&& c.GetComponent<Treasure>().GetComponent<Staff>().StuffType != StuffTypes.Junk
								select c;
				var bag = from c in player.Bag
						  where c.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff
						  && c.GetComponent<Treasure>().GetComponent<Staff>().StuffType != StuffTypes.Junk
						  select c;
				var staff = hand.Concat(inventary).Concat(bag);
				for (var i = 0; i < 3; i++)
				{
					var c = staff.ElementAt(Random.Range(1, staff.Count() - 1));
					if (c.transform.parent.gameObject.name == "Hand")
					{
						player.Hand.Remove(c);
					}
					else if (c.transform.parent.gameObject.name == "Inventary")
					{
						player.Inventary.Remove(c);
					}
					else if (c.transform.parent.gameObject.name == "Bag")
					{
						player.Bag.Remove(c);
					}
					game.tReset(c);
				}
				ToReset(card);
				break;
			case "Re_education":
				if (player.Class.Any())
				{
					var tReset = GameObject.Find("tReset").GetComponentsInChildren<Class>().ToList();
					var newClass = new Card();
					var oldClass = player.Class.FirstOrDefault();
					if (tReset.Any())
					{
						player.Class.Remove(oldClass);
						game.dReset(oldClass);
						player.Class.Add(tReset.First().GetComponentInParent<Card>());
					}
					else
					{
						foreach (var cl in player.Class)
						{
							player.Class.Remove(cl);
							game.dReset(cl);
						}
					}
				}
				ToReset(card);
				break;
			case "Torn_Backpack":
				var hnd =
					from c in player.Hand
					where c.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff
					select c;
				var inv = from c in player.Inventary
						  where c.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff
						  select c;
				var bg = from c in player.Bag
						 where c.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff
						 select c;
				var stf = hnd.Concat(inv).Concat(bg);
				for (var i = 0; i < 2; i++)
				{
					var c = stf.ElementAt(Random.Range(0, stf.Count()));
					if (c.transform.parent.gameObject.name == "Hand")
					{
						player.Hand.Remove(c);
					}
					else if (c.transform.parent.gameObject.name == "Inventary")
					{
						player.Inventary.Remove(c);
					}
					else if (c.transform.parent.gameObject.name == "Bag")
					{
						player.Bag.Remove(c);
					}
					game.tReset(c);
				}
				ToReset(card);
				break;
			case "Slave_colar":
				player.Trapped = true;
				player.Trap = card;
				break;
			case "Radioactivity_puddles":
				foreach (var i in player.Inventary)
				{
					player.Radiation = card;
					if (i.GetComponent<Staff>().StuffType == StuffTypes.Boots)
					{
						player.Inventary.Remove(i);
						game.tReset(i);
					}
				}
				ToReset(card);
				break;
			case "A_bunch_of_granades":
				foreach (var i in player.Inventary)
				{
					player.Radiation = card;
					if (i.GetComponent<Staff>().StuffType == StuffTypes.Armor)
					{
						player.Inventary.Remove(i);
						game.tReset(i);
					}
				}
				ToReset(card);
				break;
			case "Confussion":
				if (player.Class.Any())
				{
					var cls = player.Class.FirstOrDefault();
					player.Class.Remove(cls);
					game.dReset(cls);
				}
				else
					player.Level--;
				ToReset(card);
				break;
			case "Baseball_pitcher":
				foreach (var i in player.Inventary)
				{
					player.Radiation = card;
					if (i.GetComponent<Staff>().StuffType == StuffTypes.Helmet)
					{
						player.Inventary.Remove(i);
						game.tReset(i);
					}
				}
				ToReset(card);
				break;
			case "Fatal_breakage":
				var maxP = player.Inventary.Max(s => s.GetComponent<Staff>().Power);
				var st = player.Inventary.Where(s => s.GetComponent<Staff>().Power == maxP).FirstOrDefault();
				player.Inventary.Remove(st);
				game.tReset(st);
				ToReset(card);
				break;
			case "Stuck_in_the_textures":
				player.RunAbility = 0;
				player.Trapped = true;
				player.Trap = card;
				card.Selected = false;
				GameObject.FindObjectOfType<Game>().CurrentCard = null;
				break;
			case "Pick_pocketing":
				//TODO: Изменить при наличии других игроков
				var crd = player.Hand.ElementAt(Random.Range(0, player.Hand.Count - 1));
				player.Hand.Remove(crd);
				if (crd.Type == CardType.Door)
					game.dReset(crd);
				else
					game.tReset(crd);
				ToReset(card);
				break;
			case "RaidersRaid":
				//TODO: Дописать значение о лишении уровня.
				bool isRaider = false;
				foreach (var cl in player.Class)
				{
					if (cl.GetComponent<Door>().Name == "Raider")
					{
						isRaider = true;
						break;
					}
				}
				if (!isRaider)
				{
					var maxC = player.Inventary.Max(s => s.GetComponent<Staff>().Power);
					var c = player.Inventary.Where(s => s.GetComponent<Staff>().Price == maxC).FirstOrDefault();
					game.tReset(c);
				}
				ToReset(card);
				break;
		}
	}
	void ToReset(Card card)
	{
		var game = GameObject.FindObjectOfType<Game>();
		if (card.Type == CardType.Door)
			game.dReset(card);
		else
			game.tReset(card);
		card.Selected = false;
		GameObject.FindObjectOfType<Game>().CurrentCard = null;
	}
}
