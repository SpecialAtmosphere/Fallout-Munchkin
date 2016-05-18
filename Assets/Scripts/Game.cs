using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class Game : MonoBehaviour
{
	public List<Door> Doors;
	public List<Treasure> Treasures;
	public List<Perk> Perks;

	public string CurrentStage;
	public Player CurrentPlayer;
	public Card CurrentCard;
	public bool Fight;

	private string message;

	// Use this for initialization
	void Start()
	{
		InitialiseCardSteck();
		StartGame();
	}

	void OnGUI()
	{
		GUILayout.Label("Cards in deck:" + message);
	}

	[PunRPC]
	public void CardsCount(string msg)
	{
		message = msg;
	}
	//	GUILayout.Label("Current Stage: " + CurrentStage);
	//	if (CurrentPlayer != null)
	//	{
	//		GUILayout.Label("Current Player: " + CurrentPlayer.name);
	//		GUILayout.Label("Player Lvl: " + CurrentPlayer.Level);
	//		GUILayout.Label("Player Power: " + CurrentPlayer.Power);
	//		if (CurrentPlayer.Classes != null)
	//		{
	//			var classString = "";
	//			CurrentPlayer.ConditionClasses.ToList().ForEach(x => classString += x.ToString() + " ");
	//			GUILayout.Label(string.Format("Player class: {0}", classString));
	//		}
	//	}
	//	if (CurrentStage == "Radiation")
	//	{
	//		GUILayout.Label("Radiation Defense: " + CurrentPlayer.RadiationDefense);
	//	}
	//	GUILayout.Label(string.Format("Selected card: {0}", (CurrentCard != null) ? CurrentCard.name : "null"));

		//	if (CurrentStage == "Start")
		//	{
		//		if (GUI.Button(new Rect(400, 50, 125, 50), "Start game!"))
		//		{
		//			GiveCards(CurrentPlayer);
		//			CurrentStage = "Begin";
		//		}
		//	}
		//	if (CurrentStage == "Begin")
		//	{
		//		if (GUI.Button(new Rect(400, 50, 125, 50), "Open Door!"))
		//		{
		//			OpenDoor();
		//		}
		//	}
		//	if (CurrentStage == "Fight")
		//	{
		//		if (GUI.Button(new Rect(275, 50, 125, 50), "Fight!"))
		//		{
		//			FightMonster(CurrentCard);
		//		}
		//		if (GUI.Button(new Rect(400, 50, 125, 50), "Run!"))
		//		{
		//			//Прописать смывку от монстра
		//		}
		//		if (GUI.Button(new Rect(525, 50, 125, 50), "Help!"))
		//		{
		//			//ПРописать помощь друзей
		//		}
		//	}
		//	if (CurrentStage == "Take")
		//	{
		//		if (GUI.Button(new Rect(400, 50, 125, 50), "Take to hand!"))
		//		{
		//			TakeCard(CurrentCard);
		//			CurrentStage = "Hide & Seek";
		//		}
		//	}
		//	if (CurrentStage == "Radiation")
		//	{
		//		if (GUI.Button(new Rect(400, 50, 125, 50), "Throw a dice!"))
		//		{
		//			ThrowDice(CurrentCard);
		//		}
		//	}
		//	if (CurrentStage == "Hide & Seek")
		//	{
		//		if (GUI.Button(new Rect(400, 50, 125, 50), "Open door in dark!"))
		//		{
		//			OpenDoorInDark();
		//		}
		//	}
		//}

	public void GiveCards(Player player)
	{
		for (var i = 0; i < 4; i++)
		{
			var rd = Random.Range(0, Doors.Count);
			var rt = Random.Range(0, Treasures.Count);

			var door = Doors[rd].GetComponentInParent<Card>();
			player.ToHand(door);
			Doors.RemoveAt(rd);

			var trs = Treasures[rt].GetComponentInParent<Card>();
			player.ToHand(trs);
			Treasures.RemoveAt(rt);
		}
	}

	void InitialiseCardSteck()
	{
		Doors = MeshUpDoors(Resources.FindObjectsOfTypeAll<Door>().ToList());
		Treasures = MeshUpTreasures(Resources.FindObjectsOfTypeAll<Treasure>().ToList());
		Perks = MeshUpPerks(Resources.FindObjectsOfTypeAll<Perk>().ToList());
	}

	void StartGame()
	{
		CurrentStage = "Start";
	}

	private List<Door> MeshUpDoors(List<Door> doors)
	{
		int newNum;
		int oldNum = 0;
		foreach (var d in doors)
		{
			newNum = ReturnRandomNumber(doors.Count, oldNum);
			d.GetComponentInParent<Card>().Id = newNum;
			oldNum = newNum;
		}
		return doors;
	}

	private List<Treasure> MeshUpTreasures(List<Treasure> treasures)
	{
		int newNum;
		int oldNum = 0;
		foreach (var d in treasures)
		{
			newNum = ReturnRandomNumber(treasures.Count, oldNum);
			d.GetComponentInParent<Card>().Id = newNum;
			oldNum = newNum;
		}
		return treasures;
	}

	private List<Perk> MeshUpPerks(List<Perk> perks)
	{
		int newNum;
		int oldNum = 0;
		foreach (var d in perks)
		{
			newNum = ReturnRandomNumber(perks.Count, oldNum);
			d.GetComponentInParent<Card>().Id = newNum;
			oldNum = newNum;
		}
		return perks;
	}

	private int ReturnRandomNumber(int count, int oldValue)
	{
		int i = 0;
		int idl = Random.Range(1, count);
		if (idl == oldValue)
			idl = (idl + 1) % count;
		oldValue = idl;
		i += (idl + 1);
		return i;
	}

	void OpenDoor()
	{
		var door = Doors[Random.Range(1, Doors.Count)];
		Debug.Log(door.ToString());
		door.transform.Translate(15, -35, 0);
		door.transform.Rotate(0, 0, 270);
		door.transform.localScale = new Vector3(3, 3, 3);
		door.GetComponentInParent<Card>().player = CurrentPlayer;
		Card currentCard = door.GetComponentInParent<Card>();
		CurrentCard = currentCard;
		CurrentStage = "Open Door";
		StartCoroutine(WaitForSeconds());
		if (door.Type == Door.DoorType.Monster)
		{
			if (door.GetComponentInChildren<Monster>().Radiation)
			{
				CurrentStage = "Radiation";
			}
			else
			{
				CurrentStage = "Fight";
			}
		}
		else if (door.Type == Door.DoorType.Trap)
		{
			CurrentStage = "Trap";
			door.GetComponent<Trap>().Activate(CurrentPlayer);
			CurrentStage = "Finish";
		}
		else if (door.Type == Door.DoorType.Radiation)
		{
			CurrentStage = "Radiation";
		}
		else
		{
			CurrentStage = "Take";
		}
	}

	IEnumerator WaitForSeconds()
	{
		print(Time.time);
		yield return new WaitForSeconds(5);
		print(Time.time);
	}

	void OpenDoorInDark()
	{
		var door = Doors[Random.Range(1, Doors.Count)];
		door.GetComponentInParent<Card>().player = CurrentPlayer;
		Card currentCard = door.GetComponentInParent<Card>();
		CurrentCard = currentCard;
		TakeCard(currentCard);
	}

	void TakeCard(Card card)
	{
		CurrentPlayer.Hand.Add(card);
		var hand = CurrentPlayer.transform.FindChild("Hand");
		card.transform.parent = hand.transform;
		Vector3 pos = new Vector3(5 + (CurrentPlayer.Hand.Count * 5), -11, 0);
		card.transform.position = pos;
		card.transform.localScale = new Vector3(1, 1, 1);
		if (CurrentStage == "Hide & Seek")
		{
			card.transform.Rotate(0, 0, 270);
			CurrentStage = "Finish";
		}
	}

	void ThrowDice(Card card)
	{
		//Изменить на универсальный метод для кубика
		var num = Random.Range(1, 6);
		Debug.Log("Dice: " + num);
		var dice = GameObject.Find("Dice");
		var side = dice.transform.FindChild(num.ToString()).gameObject;
		side.SetActive(true);
		CurrentPlayer.RadiationDefense += num;
		if (CurrentPlayer.RadiationDefense > 6)
		{
			CurrentStage = "Finish";
			dReset(card);
		}
		else
		{
			//TODO:Если тип двери "Радиация"
			card.GetComponent<Radiation>().Acivate(CurrentPlayer);
			CurrentStage = "Finish";
		}
	}

	public void tReset(Card card)
	{
		var tReset = GameObject.Find("tReset");
		card.transform.parent = tReset.transform;
		card.transform.Rotate(0, 0, 270);
		card.transform.localScale = new Vector3(1, 1, 1);
		card.transform.position = new Vector3(-19, -30, 0);
		card.Deselect();
		card.SwitchGameLocation(GameLocations.InReset);
	}

	public void dReset(Card card)
	{
		var dReset = GameObject.Find("dReset");
		Debug.Log(card.transform.parent.ToString() + ", " + dReset.transform.ToString());
		card.transform.parent = dReset.transform;
		card.transform.Rotate(0, 0, 270);
		card.transform.localScale = new Vector3(1, 1, 1);
		card.transform.position = new Vector3(-19, -30, 0);
		card.Deselect();
		card.SwitchGameLocation(GameLocations.InReset);
	}

	public void pReset(Card card)
	{
		//TODO: Сделать сброс для перков
		card.Deselect();
		card.SwitchGameLocation(GameLocations.InReset);
	}

	public void FightMonster(Card card)
	{
		if (CurrentPlayer.Power < card.GetComponent<Monster>().Level)
		{
			var trs = GameObject.Find("Treasures").GetComponentsInChildren<Card>().ToList();
			for (var i = 0; i < card.GetComponent<Monster>().TreasureCount; i++)
			{
				var ind = Random.Range(1, trs.Count);
				CurrentPlayer.Hand.Add(trs.ElementAt(ind));
				var hand = CurrentPlayer.transform.FindChild("Hand");
				card.transform.parent = hand.transform;
			}
		}
	}
}
