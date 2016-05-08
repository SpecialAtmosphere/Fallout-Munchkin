using UnityEngine;

abstract class ConditionModifier : SimpleModifier
{
	public Game game;

	/// <summary>
	/// Условие, определяющее прибавление к основному бонусу дополнительного
	/// </summary>
	/// <returns></returns>
	protected abstract bool SatisfiesCondition();

	void Start()
	{
		game = GameObject.FindObjectOfType<Game>();
	}
}
