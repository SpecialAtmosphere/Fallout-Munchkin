class AgainstDeadlyModifier : AdditionalBonusModifier
{
	protected override bool SatisfiesCondition()
	{
		// смерть в непотребстве
		return false;
	}
}
