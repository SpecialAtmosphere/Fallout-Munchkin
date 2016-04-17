using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CardPlayer : MonoBehaviour, ICardPlayer
{
	public List<Card> Hand;
	public List<Card> Inventary;
	public List<Card> Bag;
	public List<Card> Perks;
	public List<Card> Class;
	public List<Card> Partners;

	/// <summary>
	/// Минимальный уровень персонажа, ниже которого опуститься нельзя
	/// </summary>
	const int defaulMinLevel = 1;
	/// <summary>
	/// Кол-во напарников персонажа по умолчанию
	/// </summary>
	const int defaultPartnersCount = 1;
	/// <summary>
	/// Кол-во рук персонажа по умолчанию
	/// </summary>
	const int defaultHandCount = 2;

	public bool CanTakePartner
	{
		get
		{
			return defaultPartnersCount - Partners.Count > 0 /* + бонусы из модификаторов */;
		}
	}

	/// <summary>
	/// Текущий уровень персонажа
	/// </summary>
	int level = defaulMinLevel;
	public int Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value > defaulMinLevel ? value : defaulMinLevel;
		}
	}
	/// <summary>
	/// Текущая сила персонажа
	/// </summary>
	public int Power
	{
		get
		{
			return Level + Modifiers.Sum(x => x.Bonus);
		}
	}

	public IEnumerable<IModifier> Modifiers
	{
		get
		{
			return Inventary.Select(x => x.GetComponent<Staff>()).Cast<IModifier>();
		}
	}

	public bool isSuper = false;
	public bool isMegaBrain = false;

	public int RunAbility = 2;

	public int RadiationDefense = 1;
	public bool Radiated;
	public Card Radiation;

	public bool Trapped;
	public Card Trap;

	public bool isKilled;

	public void Killed()
	{
		var game = GameObject.FindObjectOfType<Game>();
		foreach (var t in this.Inventary)
		{
			this.Inventary.Remove(t);
			game.tReset(t);
		}
		foreach (var b in this.Bag)
		{
			this.Bag.Remove(b);
			game.tReset(b);
		}
		foreach (var h in this.Hand)
		{
			this.Hand.Remove(h);
			if (h.Type == Card.CardType.Door)
				game.dReset(h);
			else
				game.tReset(h);
		}
		foreach (var p in this.Perks)
		{
			this.Perks.Remove(p);
			game.pReset(p);
		}
		foreach (var c in this.Class)
		{
			this.Class.Remove(c);
			game.dReset(c);
		}
		isSuper = false;
		isMegaBrain = false;
		RadiationDefense = 1;
		RunAbility = 2;
		Radiated = false;
		Radiation = null;
		isKilled = true;
	}
}
