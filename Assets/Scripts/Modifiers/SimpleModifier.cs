using UnityEngine;

/// <summary>
/// Модификатор прямого отображения заданных значений (без логики)
/// </summary>
class SimpleModifier : MonoBehaviour, IModifier
{
	public int bonus = 0;
	public bool ignore = false;
	public int radiationDefenceMod = 0;
	public int ranAwayMod = 0;
	public int handCountMod = 0;
	public int perkCountMod = 0;
	public int bigStuffMaxCountMod = 0;
	public int partnerMaxCapacityMod = 0;
	public int classCapacityMod = 0;
	public int perksToPickFromCountMod = 0;
	public ConditionClass givesClass = ConditionClass.No;
	public ConditionRadiation givesRadClass = ConditionRadiation.No;
	public int thingsToGetAfterDeathMod = 0;

	/// <summary>
	/// Бонус, который будет добавлен к силе стороны (манчкин/монстр)
	/// </summary>
	public virtual int Bonus
	{
		get
		{
			return bonus;
		}
	}
	/// <summary>
	/// Игнорируется ли модификатор
	/// </summary>
	public virtual bool Ignore
	{
		get
		{
			return ignore;
		}
	}
	/// <summary>
	/// Модификатор защиты от радиации
	/// </summary>
	public virtual int RadiationDefenceMod
	{
		get
		{
			return radiationDefenceMod;
		}
	}
	/// <summary>
	/// Модификатор смывки
	/// </summary>
	public virtual int RanAwayMod
	{
		get
		{
			return ranAwayMod;
		}
	}
	/// <summary>
	/// Модификатор числа рук
	/// </summary>
	public virtual int HandCountMod
	{
		get
		{
			return handCountMod;
		}
	}
	/// <summary>
	/// Модификатор числа перков
	/// </summary>
	public virtual int PerkCountMod
	{
		get
		{
			return perkCountMod;
		}
	}
	/// <summary>
	/// Модификатор максимального числа БОЛЬШИХ шмоток, которые могут находится в игре у игрока
	/// </summary>
	public virtual int BigStuffMaxCapacityMod
	{
		get
		{
			return bigStuffMaxCountMod;
		}
	}
	/// <summary>
	/// Модификатор максимального числа напарников
	/// </summary>
	public virtual int PartnerMaxCapacityMod
	{
		get
		{
			return partnerMaxCapacityMod;
		}
	}

	public virtual int ClassCapacityMod
	{
		get
		{
			return classCapacityMod;
		}
	}
	public virtual int PerksToPickFromCountMod
	{
		get
		{
			return perksToPickFromCountMod;
		}
	}
	public virtual ConditionClass GivesClass
	{
		get
		{
			return givesClass;
		}
	}
	public virtual ConditionRadiation GivesRadClass
	{
		get
		{
			return givesRadClass;
		}
	}
	/// <summary>
	/// Модификатор кол-ва взятых шмоток при мародёрстве
	/// </summary>
	public virtual int ThingsToGetAfterDeathMod
	{
		get
		{
			return thingsToGetAfterDeathMod;
		}
	}
}