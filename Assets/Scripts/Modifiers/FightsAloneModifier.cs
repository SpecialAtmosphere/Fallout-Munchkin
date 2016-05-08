class FightsAloneModifier : AdditionalBonusModifier
{
	protected override bool SatisfiesCondition()
	{
		// проверить, что никто не помогает в бою
		return false;
	}
}
