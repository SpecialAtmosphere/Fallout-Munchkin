using UnityEngine;
using System.Collections;

public enum GameLocations
{
	InDeck,
	InHand,
	InUse,
	InBag,
	InReset
}

public class Card : MonoBehaviour
{
	public int Id;
	public CardType Type;
	public bool Enabled;
	public bool Selected;
	public CardPlayer player;
	public GameLocations GameLocation;

	private Game game;
	private Handler Handler;
	private SpriteRenderer SpriteRenderer;
	private string preSelectedSortingLayer;

	public enum CardType
	{
		Door,
		Treasure,
		Perk
	}

	void Start()
	{
		game = GameObject.FindObjectOfType<Game>();
		GameLocation = GameLocations.InDeck;
		SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnMouseDown()
	{
		Debug.Log("Click " + this.name);
		if (!Selected)
			Select();
		else
			Deselect();
	}

	public void SwitchGameLocation(GameLocations destination)
	{
		GameLocation = destination;
		var destinationLayer = "";
		switch (destination)
		{
			case GameLocations.InBag:
			case GameLocations.InUse:
				destinationLayer = "InGame";
				break;
			case GameLocations.InDeck:
			case GameLocations.InReset:
				destinationLayer = "Deck";
				break;
			case GameLocations.InHand:
				destinationLayer = "InHand";
				break;
		}

		SpriteRenderer.sortingLayerName = destinationLayer;
	}

	public void Select()
	{
		preSelectedSortingLayer = SpriteRenderer.sortingLayerName;
		if (game.CurrentCard != null)
			game.CurrentCard.Deselect();
		game.CurrentCard = this;
		player = game.CurrentPlayer;
		this.transform.localScale = new Vector3(4, 4, 4);
		if (this.transform.parent.gameObject.name == "Bag")
			this.transform.Rotate(0, 0, 90);
		Debug.Log(this.transform.parent.gameObject.name);
		Selected = true;
		SpriteRenderer.sortingLayerName = "Selected";
	}

	public void Deselect()
	{
		SpriteRenderer.sortingLayerName = preSelectedSortingLayer;
		game.CurrentCard = null;
		this.transform.localScale = new Vector3(1, 1, 1);
		Selected = false;
	}

	void OnGUI()
	{
		if (Selected)
		{
			//Button Use
			if (GUI.Button(new Rect(400, 400, 125, 50), "Use"))
			{
				Use(this);
			}
			//Stage Finish
			if (game.CurrentStage == "Finish")
			{
				if (GUI.Button(new Rect(275, 400, 125, 50), "Share"))
				{
					//TODO: Отдать слабому игроку
				}
				if (GUI.Button(new Rect(525, 400, 125, 50), "Drop"))
				{
					game.tReset(this);
				}
			}
			//Treasure
			if (Type == CardType.Treasure)
			{
				if (GetComponent<Treasure>().Type == Treasure.TreasureType.Staff)
				{
					var stuff = GetComponent<Staff>();
					switch (stuff.StuffType)
					{
						case StuffTypes.Weapon:
						case StuffTypes.Knuckles:
						case StuffTypes.Helmet:
						case StuffTypes.Armor:
						case StuffTypes.Boots:
							if (GUI.Button(new Rect(400, 450, 125, 50), "Put On"))
								PutOn(this);
							break;
						default:
						case StuffTypes.None:
						case StuffTypes.Modifier:
						case StuffTypes.Junk:
						case StuffTypes.Explosive:
							break;
					}

					if (GUI.Button(new Rect(525, 450, 125, 50), "To Bag"))
						ToBag(this);
					if (GUI.Button(new Rect(275, 450, 125, 50), "Sell"))
					{
						//TODO: Продать из корзины
					}
				}
			}
		}
	}
	//Метод для использования карты всех типов
	void Use(Card card)
	{
		player = game.CurrentPlayer;
		//Тип двери
		if (card.Type == CardType.Door)
		{
			var door = card.GetComponent<Door>();
			//Подтип Класс
			if (card.GetComponent<Door>().Type == Door.DoorType.Class)
			{
				door.GetComponent<Class>().Activate(player);
			}
			//Подтип Напарник
			if (card.GetComponent<Door>().Type == Door.DoorType.Partner)
			{
				door.GetComponent<Partner>().Activate(player);
			}
			//Подтип Радиация
			if (card.GetComponent<Door>().Type == Door.DoorType.Radiation)
			{
				//TODO: Использование радиации с руки на другого игрока
			}
			//Подтип Ловушка
			if (card.GetComponent<Door>().Type == Door.DoorType.Trap)
			{
				//TODO: Использование ловушки с руки на другого игрока
			}
		}
		//Тип сокровища
		if (card.Type == CardType.Treasure)
		{
			//подтип Лвл
			if (card.GetComponent<Treasure>().Type == Treasure.TreasureType.Lvl)
			{
				card.GetComponent<Treasure>().Use(player);
			}
			//подтип Взрывчатка
			if (card.GetComponent<Staff>().StuffType == StuffTypes.Explosive)
			{
				//TODO: Вызвать метод из словаря
				card.GetComponent<Treasure>().Use(player);
			}
			//подтип Бафф
			if (card.GetComponent<Staff>().StuffType == StuffTypes.None)
			{
				//TODO: Вызвать метод из словаря
				card.GetComponent<Treasure>().Use(player);
			}
		}
	}
	//Метод надеть карту
	void PutOn(Card card)
	{
		if (card.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff)
		{
			card.GetComponent<Treasure>().GetComponent<Staff>().PutOn(player);
		}
	}

	void ToBag(Card card)
	{
		player = game.CurrentPlayer;
		if (card.Type == CardType.Treasure)
		{
			if (card.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff)
			{
				var trs = card.GetComponent<Treasure>();
				var staff = trs.GetComponent<Staff>();
				staff.ToBag(player);
			}
		}
	}
}
