using System.Collections.Generic;

public interface IItemBox
{
	ICollection<Card> Stack { get; set; }
	ItemBox currentBox { get; set; }
}
