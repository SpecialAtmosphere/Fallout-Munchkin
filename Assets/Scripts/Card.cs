using UnityEngine;
using UnityEngine.EventSystems;

public enum GameLocations
{
	InDeck,
	InHand,
	InUse,
	InBag,
	InReset
}

public enum CardType
{
	Door,
	Treasure,
	Perk
}

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	/// <summary>
	/// Публичные параметры
	/// </summary>
	public int Id;
	public CardType Type;
	public bool Enabled;
	public bool Selected;
	public Player player;
	public GameLocations GameLocation;
	public ItemBox box;

	/// <summary>
	/// Частные параметры
	/// </summary>
	private Game game;
	private Handler Handler;
	private SpriteRenderer SpriteRenderer;
	private string preSelectedSortingLayer;
	private Vector2 offset;
	private PhotonView photonView;

	/// <summary>
	/// Событие добавления карты
	/// </summary>
	public delegate void CardAdd(Card card);
	public static event CardAdd OnCardAdded;

	/// <summary>
	/// Событие удаление карты
	/// </summary>
	public delegate void CardRemove(Card card);
	public static event CardRemove OnCardRemoved;

	void Start()
	{
		game = FindObjectOfType<Game>();
		GameLocation = GameLocations.InDeck;
		SpriteRenderer = GetComponent<SpriteRenderer>();
		box = GetComponentInParent<ItemBox>();
		photonView = GetComponent<PhotonView>();
		photonView.synchronization = ViewSynchronization.Off;
	}
	/// <summary>
	/// Событие: Начало перетаскивания карты
	/// </summary>
	/// <param name="eventData"></param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		if(photonView.owner != null)
			Debug.Log(photonView.owner.name);
		Debug.Log("photonView.isMine: " + photonView.isMine);
		if (photonView.isMine)
		{
			offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
			transform.SetParent(transform.parent.parent);
			transform.position = eventData.position;
			GetComponent<CanvasGroup>().blocksRaycasts = false;
			if (Selected)
				Deselect();
			OnCardRemoved(this);
		}
	}
	/// <summary>
	/// Событие: перетаскивание карты
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position - offset;
	}

	//TODO: Написать проверку при броске карты в ячейку, 
	//есле условие не совпадает, то возвращать на предыдущее место

	/// <summary>
	/// Событие: Бросок карты в ячейку
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		OnCardAdded(this);
		if (box != null)
		{
			switch (box.name)
			{
				case "pnl_footer":
					player = box.transform.parent.parent.GetComponent<Player>();
					player.ToHand(this);
					break;
				case "pnl_bag":
					player = box.transform.parent.parent.GetComponent<Player>();
					player.ToBag(this);
					break;
				case "pnl_inventory":
					player = box.transform.parent.parent.GetComponent<Player>();
					player.ToInventory(this);
					break;
				default:
					transform.SetParent(box.transform);
					transform.position = box.transform.position;
					break;
			}
			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
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
		if (game.CurrentCard != null)
			game.CurrentCard.Deselect();
		game.CurrentCard = this;
		transform.localScale = new Vector3(4, 4, 4);
		Selected = true;
	}

	public void Deselect()
	{
		game.CurrentCard = null;
		transform.localScale = new Vector3(1, 1, 1);
		Selected = false;
	}
	[PunRPC]
	public void ChangeCardLocation(int id, int playerId, string location, GameLocations gameLocation)
	{
		if (Id == id)
		{
			var player = PhotonView.Find(playerId).GetComponent<Player>();
			var parent = player.transform.FindDeepChild(location);
			transform.SetParent(parent);
			GameLocation = gameLocation;
		}
	}

	//void OnGUI()
	//{
	//	if (Selected)
	//	{
	//		//Button Use
	//		if (GUI.Button(new Rect(400, 400, 125, 50), "Use"))
	//		{
	//			Use(this);
	//		}
	//		//Stage Finish
	//		if (game.CurrentStage == "Finish")
	//		{
	//			if (GUI.Button(new Rect(275, 400, 125, 50), "Share"))
	//			{
	//				//TODO: Отдать слабому игроку
	//			}
	//			if (GUI.Button(new Rect(525, 400, 125, 50), "Drop"))
	//			{
	//				game.tReset(this);
	//			}
	//		}
	//		//Treasure
	//		if (Type == CardType.Treasure)
	//		{
	//			if (GetComponent<Treasure>().Type == Treasure.TreasureType.Staff)
	//			{
	//				var stuff = GetComponent<Staff>();
	//				switch (stuff.StuffType)
	//				{
	//					case StuffTypes.Weapon:
	//					case StuffTypes.Knuckles:
	//					case StuffTypes.Helmet:
	//					case StuffTypes.Armor:
	//					case StuffTypes.Boots:
	//						if (GUI.Button(new Rect(400, 450, 125, 50), "Put On"))
	//							PutOn(this);
	//						break;
	//					default:
	//					case StuffTypes.None:
	//					case StuffTypes.Modifier:
	//					case StuffTypes.Junk:
	//					case StuffTypes.Explosive:
	//						break;
	//				}

	//				if (GUI.Button(new Rect(525, 450, 125, 50), "To Bag"))
	//					ToBag(this);
	//				if (GUI.Button(new Rect(275, 450, 125, 50), "Sell"))
	//				{
	//					//TODO: Продать из корзины
	//				}
	//			}
	//		}
	//	}
	//}

	//Метод для использования карты всех типов
	//void Use(Card card)
	//{
	//	player = game.CurrentPlayer;
	//	//Тип двери
	//	if (card.Type == CardType.Door)
	//	{
	//		var door = card.GetComponent<Door>();
	//		//Подтип Класс
	//		if (card.GetComponent<Door>().Type == Door.DoorType.Class)
	//		{
	//			door.GetComponent<Class>().Activate(player);
	//		}
	//		//Подтип Напарник
	//		if (card.GetComponent<Door>().Type == Door.DoorType.Partner)
	//		{
	//			door.GetComponent<Partner>().Activate(player);
	//		}
	//		//Подтип Радиация
	//		if (card.GetComponent<Door>().Type == Door.DoorType.Radiation)
	//		{
	//			//TODO: Использование радиации с руки на другого игрока
	//		}
	//		//Подтип Ловушка
	//		if (card.GetComponent<Door>().Type == Door.DoorType.Trap)
	//		{
	//			//TODO: Использование ловушки с руки на другого игрока
	//		}
	//	}
	//	//Тип сокровища
	//	if (card.Type == CardType.Treasure)
	//	{
	//		//подтип Лвл
	//		if (card.GetComponent<Treasure>().Type == Treasure.TreasureType.Lvl)
	//		{
	//			card.GetComponent<Treasure>().Use(player);
	//		}
	//		//подтип Взрывчатка
	//		if (card.GetComponent<Staff>().StuffType == StuffTypes.Explosive)
	//		{
	//			//TODO: Вызвать метод из словаря
	//			card.GetComponent<Treasure>().Use(player);
	//		}
	//		//подтип Бафф
	//		if (card.GetComponent<Staff>().StuffType == StuffTypes.None)
	//		{
	//			//TODO: Вызвать метод из словаря
	//			card.GetComponent<Treasure>().Use(player);
	//		}
	//	}
	//}

}
