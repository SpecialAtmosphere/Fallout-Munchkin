abstract class IgnoreIfConditionModifier : ConditionModifier
{
	public override bool Ignore
	{
		get
		{
			return SatisfiesCondition() ? true : base.Ignore;
		}
	}
}
