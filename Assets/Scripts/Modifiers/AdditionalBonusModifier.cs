abstract class AdditionalBonusModifier : ConditionModifier
{
	public int additionalBonus;

	public override int Bonus
	{
		get
		{
			return SatisfiesCondition() ? base.Bonus + additionalBonus : base.Bonus;
		}
	}
}
