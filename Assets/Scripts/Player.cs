using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour, IPlayer
{
	public List<Card> Hand;
	public List<Card> Inventary;
	public List<Card> Bag;
	public List<Card> Classes;
	public List<Card> Perks;
	public List<Card> Partners;
	
	/// <summary>
	/// Компонент для передачи события PUN
	/// </summary>
	public PhotonView gameView;

	/// <summary>
	/// Текущая игра
	/// </summary>
	private Game game;

	/// <summary>
	/// Инициализация игрока
	/// </summary>
	void Start()
	{	
		game = FindObjectOfType<Game>();
		Debug.Log("Cards was: " + (game.Doors.Count + game.Treasures.Count));
		game.GiveCards(this);
		Debug.Log("Cards now: " + (game.Doors.Count + game.Treasures.Count));
		//game.GetComponent<PhotonView>().RPC("CardsCount", PhotonTargets.All, (game.Doors.Count + game.Treasures.Count));
	}

	
	public IEnumerable<ConditionClass> ConditionClasses
	{
		get
		{
			return Classes.Select(x => x.GetComponent<IModifier>()).Where(x => x.GivesClass != ConditionClass.No).Select(x => x.GivesClass);
		}
	}

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
	/// <summary>
	/// Кол-во больших шмоток, которые может нести персонаж
	/// </summary>
	const int defaultBigStuffCapacity = 1;
	/// <summary>
	/// Кол-во классов, к которым может принадлежать персонаж
	/// </summary>
	const int defaultClassCapacity = 1;

	public bool CanTakeBigStuff
	{
		get
		{
			return defaultBigStuffCapacity - Inventary.Concat(Bag).Select(x => x.gameObject.GetComponent<Staff>()).Where(x => x.BigStaff).Count() > 0; //+ бонусы
		}
	}

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
			var inventary = Inventary.Select(x => x.GetComponent<IModifier>());
			var classes = Classes.Select(x => x.GetComponent<IModifier>());
			return inventary.Concat(classes);
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
		foreach (var t in Inventary)
		{
			Inventary.Remove(t);
			game.tReset(t);
		}
		foreach (var b in Bag)
		{
			Bag.Remove(b);
			game.tReset(b);
		}
		foreach (var h in Hand)
		{
			Hand.Remove(h);
			if (h.Type == CardType.Door)
				game.dReset(h);
			else
				game.tReset(h);
		}
		foreach (var p in Perks)
		{
			Perks.Remove(p);
			game.pReset(p);
		}
		isSuper = false;
		isMegaBrain = false;
		RadiationDefense = 1;
		RunAbility = 2;
		Radiated = false;
		Radiation = null;
		isKilled = true;
	}

	/// <summary>
	/// Генерирует событие: добавление класса игрока
	/// </summary>
	/// <param name="card"></param>
	public delegate void ClassChange(Card card);
	public static event ClassChange OnClassChanged;

	/// <summary>
	/// Подписывание на событии
	/// </summary>
	void OnEnable()
	{
		Card.OnCardAdded += AddCard;
		Card.OnCardRemoved += RemoveCard;
	}
	/// <summary>
	/// Отписывание от событий
	/// </summary>
	void OnDisable()
	{
		Card.OnCardAdded -= AddCard;
		Card.OnCardRemoved -= RemoveCard;
	}
	
	/// <summary>
	/// Метод подписаный на событие добавление карты из "руки". Необязательный.
	/// </summary>
	/// <param name="card"></param>
	void AddCard(Card card)
	{
		//Debug.Log(card.name + " added, Box name: " + card.box.name);
	}

	/// <summary>
	/// Метод подписаный на событие удаление карты
	/// </summary>
	/// <param name="card"></param>
	void RemoveCard(Card card)
	{
		//Debug.Log(card.name + " removed");
		if (Hand.Exists(x => x == card))
		{
			Hand.Remove(card);
			RefreshHand();
		}
		else if (Bag.Exists(x => x == card))
		{
			Bag.Remove(card);
			RefreshBag();
		}
		else if (Inventary.Exists(x => x == card))
		{
			Inventary.Remove(card);
		}
		else if (Classes.Exists(x => x == card))
		{
			Classes.Remove(card);
		}
		else if (Perks.Exists(x => x == card))
		{
			Perks.Remove(card);
		}
		else if (Partners.Exists(x => x == card))
		{
			Partners.Remove(card);
		}
	}

	/// <summary>
	/// Добавление карты в "Руку"
	/// </summary>
	/// <param name="card"></param>
	public void ToHand(Card card)
	{
		
		var pnl_footer = transform.FindDeepChild("pnl_footer");
		var gl_hand = pnl_footer.FindChild("gl_hand");
		var first_card_position = gl_hand.FindChild("first_card_position");
		var x = (Hand.Any()) ? 25 : 0;
		var last_card_pos = (Hand.Any()) ? Hand.Last().transform.position : first_card_position.position;
		card.GetComponent<PhotonView>().RPC("ChangeCardLocation", PhotonTargets.All, new object[] { card.Id, GetComponent<PhotonView>().viewID, "gl_hand", GameLocations.InHand });
		Hand.Add(card);
		card.GameLocation = GameLocations.InHand;
		card.transform.SetParent(gl_hand.transform);
		card.box = pnl_footer.GetComponent<ItemBox>();
		card.transform.position = new Vector2(last_card_pos.x + x, first_card_position.position.y);
	}

	/// <summary>
	/// Добавление карты в сумку
	/// </summary>
	/// <param name="card"></param>
	public void ToBag(Card card)
	{
		var pnl_bag = transform.FindDeepChild("pnl_bag");
		var gl_bag = pnl_bag.FindChild("gl_bag");
		var first_card_position = gl_bag.FindChild("first_card_position");
		var y = (Bag.Any()) ? 35 : 0;
		var last_card_pos = (Bag.Any()) ? Bag.Last().transform.position : first_card_position.transform.position;
		Bag.Add(card);
		card.GameLocation = GameLocations.InBag;
		card.transform.SetParent(gl_bag);
		card.box = pnl_bag.GetComponent<ItemBox>();
		card.transform.position = new Vector2(first_card_position.position.x, last_card_pos.y - y);
	}
	/// <summary>
	/// Добавление карты в инвентарь
	/// </summary>
	/// <param name="card"></param>
	public void ToInventory(Card card)
	{
		var pnl_inventory = transform.FindDeepChild("pnl_inventory");
		var box_class = pnl_inventory.FindDeepChild("box_class");
		var box_partner = pnl_inventory.FindDeepChild("box_partner");
		var box_perks = pnl_inventory.FindDeepChild("box_perks");
		var box_head = pnl_inventory.FindDeepChild("box_head");
		var box_armor = pnl_inventory.FindDeepChild("box_armor");
		var box_boots = pnl_inventory.FindDeepChild("box_boots");
		var box_weapon = pnl_inventory.FindDeepChild("box_weapon");
		card.GameLocation = GameLocations.InUse;
		switch (card.Type)
		{
			case CardType.Door:
				var d = card.GetComponent<Door>();
				if (d.Type == Door.DoorType.Class) {
					card.transform.position = GetFirstCardPosition(box_class).position;
					card.transform.SetParent(box_class);
					Classes.Add(card);
					OnClassChanged(card);
				}
				else {
					card.transform.position = GetFirstCardPosition(box_partner).position;
					card.transform.SetParent(box_partner);
					Partners.Add(card);
				}
				break;
			case CardType.Treasure:
				Inventary.Add(card);
				var staff = card.GetComponent<Treasure>().GetComponent<Staff>();
				switch (staff.StuffType)
				{
					case StuffTypes.Armor:
						card.transform.position = GetFirstCardPosition(box_armor).position;
						card.transform.SetParent(box_armor);
						break;
					case StuffTypes.Helmet:
						card.transform.position = GetFirstCardPosition(box_head).position;
						card.transform.SetParent(box_head);
						break;
					case StuffTypes.Boots:
						card.transform.position = GetFirstCardPosition(box_boots).position;
						card.transform.SetParent(box_boots);
						break;
					case StuffTypes.Weapon:
					case StuffTypes.Knuckles:
						card.transform.position = GetFirstCardPosition(box_weapon).position;
						card.transform.SetParent(box_weapon);
						break;
				}
				break;
			case CardType.Perk:
				card.transform.position = GetFirstCardPosition(box_perks).position;
				card.transform.SetParent(box_perks);
				Perks.Add(card);
				break;
		}
	}

	private Transform GetFirstCardPosition(Transform box)
	{
		return box.FindChild("first_card_position");
	}

	/// <summary>
	/// Перетасовывает карты в "Руке" при добавлении/удалении карты
	/// </summary>
	void RefreshHand()
	{
		var gl_hand = transform.FindDeepChild("gl_hand");
		var first_card_position = gl_hand.FindChild("first_card_position");
		foreach (var c in Hand)
		{
			c.transform.position = first_card_position.position;
		}
		for (int i = 0; i < Hand.Count; i++)
		{
			var x = i * 25;
			Hand[i].transform.position = new Vector2(first_card_position.position.x + x, first_card_position.position.y);
		}
	}

	/// <summary>
	/// Перетасовывает карты в сумке при добавлении/удалении карты
	/// </summary>
	void RefreshBag()
	{
		var gl_bag = transform.FindDeepChild("gl_bag");
		var first_card_position = gl_bag.FindChild("first_card_position");
		foreach (var c in Bag)
		{
			c.transform.position = first_card_position.transform.position;
		}
		for (int i = 0; i < Bag.Count; i++)
		{
			var y = i * 35;
			Bag[i].transform.position = new Vector2(first_card_position.position.x, first_card_position.position.y - y);
		}
	}
}
