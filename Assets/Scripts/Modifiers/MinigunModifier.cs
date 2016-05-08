using System;

class MinigunModifier : AdditionalBonusModifier
{
	int additionalBonusSum;

	protected override bool SatisfiesCondition()
	{
		// доп. бонус за каждого дополнительного монстра = x;
		// для СУПЕРМУТАНТА: доп. бонус за каждого монстра в бою = x
		// additionalBonusSum = this.additionalBonus * x;
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
