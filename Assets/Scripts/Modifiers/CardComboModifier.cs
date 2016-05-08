using System.Collections.Generic;

class CardComboModifier : AdditionalBonusModifier
{
	public List<Card> comboSet;

	protected override bool SatisfiesCondition()
	{
		return false;
	}
}
