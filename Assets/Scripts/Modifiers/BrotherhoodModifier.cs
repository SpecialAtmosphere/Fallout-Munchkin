class BrotherhoodModifier : AdditionalBonusModifier
{
	int additionalBonusSum;

	protected override bool SatisfiesCondition()
	{
		//additionalBonusSum = this.game.[кол - во паладинов] * this.additionalBonus;
		return false;
	}

	public override int Bonus
	{
		get
		{
			return SatisfiesCondition() ? base.Bonus + additionalBonusSum : base.Bonus;
		}
	}
}
