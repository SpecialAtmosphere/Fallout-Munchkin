using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StuffTypes
{
	None,
	Weapon,
	Helmet,
	Armor,
	Boots,
	Modifier,
	Knuckles,
	Explosive,
	Junk
}

public class Staff : MonoBehaviour, IModifier
{
	public StuffTypes StuffType;
	public int Price;
	public int Power;
	public bool OneTime;
	public ActionSide Side;
	public bool BigStaff;
	public bool Ability;
	public bool isCondition;
	public ConditionClass ForClass;
	public ConditionClass NotForClass;
	public ConditionRadiation NotForRadiation;
	public bool IsPreWar;
	public List<Staff> StuffModifiers;

	public int Bonus
	{
		get
		{
			return Power;
		}
	}

	void Start()
	{
		StuffModifiers = new List<Staff>();
	}

	public enum ActionSide
	{
		No,
		Yourself,
		For_everyone
	}

	public enum ConditionClass
	{
		No,
		Raider,
		Enclave_scientist,
		Lone_wanderer,
		Brotherhood_paladin
	}

	public enum ConditionRadiation
	{
		No,
		Mutant,
		Ghoul
	}

	public void PutOn(CardPlayer player)
	{
		if (!isCondition)
		{
			var card = this.GetComponentInParent<Card>();

			if (card.GameLocation == GameLocations.InHand)
				player.Hand.Remove(card);
			else
				player.Bag.Remove(card);

			//Add to Inventary list
			player.Inventary.Add(card);
			card.SwitchGameLocation(GameLocations.InUse);

			//transform
			var inv = player.transform.FindChild("Inventory");
			card.transform.parent = inv.transform;
			card.transform.position = setPosition(card);
			card.transform.localScale = new Vector3(1, 1, 1);
			card.Deselect();
		}
	}

	public void ToBag(CardPlayer player)
	{
		var card = this.GetComponentInParent<Card>();

		if (card.GameLocation == GameLocations.InHand)
			player.Hand.Remove(card);
		else
			player.Inventary.Remove(card);

		player.Bag.Add(card);
		card.SwitchGameLocation(GameLocations.InBag);

		int Y = 35 - player.Bag.Count * 5;
		var bag = player.transform.FindChild("Bag");
		card.transform.parent = bag.transform;
		card.transform.position = new Vector3(50, Y, 0);
		card.transform.Rotate(0, 0, 270);
		card.transform.localScale = new Vector3(1, 1, 1);
		card.Deselect();
	}

	private Vector3 setPosition(Card card)
	{
		Vector3 position = new Vector3(0, 0, 0);
		switch (card.GetComponent<Staff>().StuffType)
		{
			//TODO Проверка на двуручное + наличие
			case StuffTypes.Weapon:
				if (card.GetComponent<Weapon>().InTwoArms)
					position = new Vector3(17, 29, 0);
				else
					position = new Vector3(14, 29, 0);
				break;
			case StuffTypes.Knuckles:
				position = new Vector3(14, 29, 0);
				break;
			case StuffTypes.Armor:
				position = new Vector3(35, 29, 0);
				break;
			case StuffTypes.Helmet:
				position = new Vector3(28, 29, 0);
				break;
			case StuffTypes.Boots:
				position = new Vector3(41, 29, 0);
				break;
		}
		return position;
	}
}
