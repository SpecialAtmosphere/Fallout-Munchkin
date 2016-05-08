class BuffoutModifier : AdditionalBonusModifier
{
	protected override bool SatisfiesCondition()
	{
#warning при многопользовательской игре переиначить CurrentPlayer
		additionalBonus = game.CurrentPlayer.Level;
		return true;
	}
}
