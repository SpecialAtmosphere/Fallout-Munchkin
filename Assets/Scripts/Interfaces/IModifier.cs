public interface IModifier
{
	/// <summary>
	/// Бонус, который будет добавлен к силе стороны (манчкин/монстр)
	/// </summary>
	int Bonus { get; }
	/// <summary>
	/// Игнорируется ли модификатор
	/// </summary>
	bool Ignore { get; }
	/// <summary>
	/// Модификатор защиты от радиации
	/// </summary>
	int RadiationDefenceMod { get; }
	/// <summary>
	/// Модификатор смывки
	/// </summary>
	int RanAwayMod { get; }
	/// <summary>
	/// Модификатор числа рук
	/// </summary>
	int HandCountMod { get; }
	/// <summary>
	/// Модификатор числа перков
	/// </summary>
	int PerkCountMod { get; }
	/// <summary>
	/// Модификатор максимального числа БОЛЬШИХ шмоток, которые могут находится в игре у игрока
	/// </summary>
	int BigStuffMaxCapacityMod { get; }
	/// <summary>
	/// Модификатор максимального числа напарников
	/// </summary>
	int PartnerMaxCapacityMod { get; }
	/// <summary>
	/// Модификатор кол-ва классов, к которым может принадлежать персонаж
	/// </summary>
	int ClassCapacityMod { get; }
	/// <summary>
	/// Модификатор кол-ва перков для выбора
	/// </summary>
	int PerksToPickFromCountMod { get; }
	ConditionClass GivesClass { get; }
	ConditionRadiation GivesRadClass { get; }
	/// <summary>
	/// Модификатор кол-ва взятых шмоток при мародёрстве
	/// </summary>
	int ThingsToGetAfterDeathMod { get; }
}
