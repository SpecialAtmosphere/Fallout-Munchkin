using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class Game : MonoBehaviour
{
	public string message;
	public Door[] Doors;
	public Treasure[] Treasures;
	public Perk[] Perks;
	public CardPlayer[] Players;
	public string CurrentStage;
	public CardPlayer CurrentPlayer;
	public Card CurrentCard;
	public bool Fight;

	// Use this for initialization
	void Start()
	{
		InitialiseCardSteck();
		InitializePlayers();
		StartGame();
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(0, 0, 90, 30), "Reset Lvl!"))
		{
			GameObject.FindObjectOfType<ResetLvl>().Reset();
		}
		GUILayout.Label("");
		GUILayout.Label("Current Stage: " + CurrentStage);
		GUILayout.Label("System message: " + message);
		if (CurrentPlayer != null)
		{
			GUILayout.Label("Current Player: " + CurrentPlayer.name);
			GUILayout.Label("Player Lvl: " + CurrentPlayer.Lvl);
			GUILayout.Label("Player Power: " + CurrentPlayer.Power);
			if (CurrentPlayer.Class != null)
			{
				GUILayout.Label(string.Format("Player class: {0}", (CurrentPlayer.Class.Any()) ? CurrentPlayer.Class.FirstOrDefault().name : "null"));
			}
		}
		if (CurrentStage == "Radiation")
		{
			GUILayout.Label("Radiation Defense: " + CurrentPlayer.RadiationDefense);
		}
		GUILayout.Label(string.Format("Selected card: {0}", (CurrentCard != null) ? CurrentCard.name : "null"));

		if (CurrentStage == "Start")
		{
			if (GUI.Button(new Rect(400, 50, 125, 50), "Start game!"))
			{
				GiveCards(CurrentPlayer);
				CurrentStage = "Begin";
			}
		}
		if (CurrentStage == "Begin")
		{
			if (GUI.Button(new Rect(400, 50, 125, 50), "Open Door!"))
			{
				OpenDoor();
			}
		}
		if (CurrentStage == "Fight")
		{
			if (GUI.Button(new Rect(275, 50, 125, 50), "Fight!"))
			{
				FightMonster(CurrentCard);
			}
			if (GUI.Button(new Rect(400, 50, 125, 50), "Run!"))
			{
				//Прописать смывку от монстра
			}
			if (GUI.Button(new Rect(525, 50, 125, 50), "Help!"))
			{
				//ПРописать помощь друзей
			}
		}
		if (CurrentStage == "Take")
		{
			if (GUI.Button(new Rect(400, 50, 125, 50), "Take to hand!"))
			{
				TakeCard(CurrentCard);
				CurrentStage = "Hide & Seek";
			}
		}
		if (CurrentStage == "Radiation")
		{
			if (GUI.Button(new Rect(400, 50, 125, 50), "Throw a dice!"))
			{
				ThrowDice(CurrentCard);
			}
		}
		if (CurrentStage == "Hide & Seek")
		{
			if (GUI.Button(new Rect(400, 50, 125, 50), "Open door in dark!"))
			{
				OpenDoorInDark();
			}
		}
	}

	void GiveCards(CardPlayer CurrentPlayer)
	{
		for (var i = 0; i < 4; i++)
		{
			int irnd = Random.Range(0, Doors.Length);
			int irnt = Random.Range(0, Treasures.Length);
			GiveDoors(irnd);
			GiveTreasures(irnt);
		}
		Debug.Log(CurrentPlayer.Hand.Count);
		for (var i = 0; i < CurrentPlayer.Hand.Count; i++)
		{
			int x = i + 1;
			Vector3 pos = new Vector3(5 + (x * 5), -11, 0);
			Debug.Log(i);
			CurrentPlayer.Hand[i].transform.position = pos;
			CurrentPlayer.Hand[i].transform.Rotate(0, 0, 270);
		}
	}

	void GiveDoors(int ind)
	{
		var door = Doors[ind];
		var card = door.GetComponentInParent<Card>();
		List<Door> Doorlist = Doors.ToList();
		CurrentPlayer.Hand.Add(card);
		var hand = CurrentPlayer.transform.FindChild("Hand");
		card.transform.parent = hand.transform;
		card.SwitchGameLocation(GameLocations.InHand);
		Doorlist.RemoveAt(ind);
	}

	void GiveTreasures(int ind)
	{
		var trs = Treasures[ind];
		var card = trs.GetComponentInParent<Card>();
		List<Treasure> Treasurelist = Treasures.ToList();
		CurrentPlayer.Hand.Add(trs.GetComponentInParent<Card>());
		var hand = CurrentPlayer.transform.FindChild("Hand");
		card.transform.parent = hand.transform;
		card.SwitchGameLocation(GameLocations.InHand);
		Treasurelist.RemoveAt(ind);
	}

	void InitialiseCardSteck()
	{
		Doors = MeshUpDoors(Resources.FindObjectsOfTypeAll<Door>());
		Treasures = MeshUpTreasures(Resources.FindObjectsOfTypeAll<Treasure>());
		Perks = MeshUpPerks(Resources.FindObjectsOfTypeAll<Perk>());
	}

	void InitializePlayers()
	{
		Players = Resources.FindObjectsOfTypeAll<CardPlayer>();
	}

	void StartGame()
	{
		CurrentStage = "Start";
		CurrentPlayer = Players[0];
	}

	Door[] MeshUpDoors(Door[] doors)
	{
		int newNum;
		int oldNum = 0;
		foreach (var d in doors)
		{
			newNum = ReturnRandomNumber(doors.Length, oldNum);
			d.GetComponent<SpriteRenderer>().sortingOrder = newNum;
			d.GetComponentInParent<Card>().Id = newNum;
			oldNum = newNum;
		}
		return doors;
	}

	Treasure[] MeshUpTreasures(Treasure[] treasures)
	{
		int newNum;
		int oldNum = 0;
		foreach (var d in treasures)
		{
			newNum = ReturnRandomNumber(treasures.Length, oldNum);
			d.GetComponent<SpriteRenderer>().sortingOrder = newNum;
			d.GetComponentInParent<Card>().Id = newNum;
			oldNum = newNum;
		}
		return treasures;
	}

	Perk[] MeshUpPerks(Perk[] perks)
	{
		int newNum;
		int oldNum = 0;
		foreach (var d in perks)
		{
			newNum = ReturnRandomNumber(perks.Length, oldNum);
			d.GetComponent<SpriteRenderer>().sortingOrder = newNum;
			d.GetComponentInParent<Card>().Id = newNum;
			oldNum = newNum;
		}
		return perks;
	}

	int ReturnRandomNumber(int count, int oldValue)
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
		var door = Doors[Random.Range(1, Doors.Length)];
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
		var door = Doors[Random.Range(1, Doors.Length)];
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
			CurrentPlayer.Lvl += card.GetComponent<Monster>().LevelCount;
		}
	}
}
