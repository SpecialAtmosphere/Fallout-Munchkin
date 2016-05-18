using UnityEngine.EventSystems;

public class InvItemBox : ItemBox
{
	public override void OnDrop(PointerEventData eventData)
	{
		var card = eventData.pointerDrag.GetComponent<Card>();
		if (card.Type == CardType.Perk)
		{
			base.OnDrop(eventData);
		}
		else if (card.Type == CardType.Door)
		{
			if (card.GetComponent<Door>().Type == Door.DoorType.Class || card.GetComponent<Door>().Type == Door.DoorType.Partner)
				base.OnDrop(eventData);
		}
		else if (card.Type == CardType.Treasure && card.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff)
		{
			var staff = card.GetComponent<Treasure>().GetComponent<Staff>();
			if (staff.StuffType == StuffTypes.Armor ||
				staff.StuffType == StuffTypes.Helmet ||
				staff.StuffType == StuffTypes.Boots ||
				staff.StuffType == StuffTypes.Weapon ||
				staff.StuffType == StuffTypes.Knuckles)
				base.OnDrop(eventData);
		}
	}
}
