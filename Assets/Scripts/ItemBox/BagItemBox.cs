using UnityEngine.EventSystems;

public class BagItemBox : ItemBox
{
	public override void OnDrop(PointerEventData eventData)
	{
		var card = eventData.pointerDrag.GetComponent<Card>();
		if(card.Type == CardType.Treasure && card.GetComponent<Treasure>().Type == Treasure.TreasureType.Staff)
			base.OnDrop(eventData);
	}
}
