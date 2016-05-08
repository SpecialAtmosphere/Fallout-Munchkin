class IgnoreIfAllRobots : IgnoreIfConditionModifier
{
	protected override bool SatisfiesCondition()
	{
		// проверить, все ли в бою монстры роботы
		return false;
	}
}
