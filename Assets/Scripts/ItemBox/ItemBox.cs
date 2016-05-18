using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ItemBox : MonoBehaviour, IDropHandler, IItemBox
{
	public ICollection<Card> Stack { get; set; }
	public ItemBox currentBox { get; set; }

	void Start()
	{
		currentBox = GetComponent<ItemBox>();
	}

	public virtual void OnDrop(PointerEventData eventData)
	{
		Card droppedCard = eventData.pointerDrag.GetComponent<Card>();
		droppedCard.box = currentBox;
		//Debug.Log(currentBox.name);
	}
}
