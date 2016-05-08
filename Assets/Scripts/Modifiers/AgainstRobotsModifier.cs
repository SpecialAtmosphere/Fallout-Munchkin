class AgainstRobotsModifier : AdditionalBonusModifier
{
	protected override bool SatisfiesCondition()
	{
		/* родительский this.game, даёт доступ к игре
		 * здесь уже можно понть поведение доп. бонуса */
		return false;
	}
}
