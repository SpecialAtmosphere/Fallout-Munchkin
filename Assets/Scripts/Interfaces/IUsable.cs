/// <summary>
/// Интерфейс проверки ввода карты в игру
/// </summary>
public interface IUsable
{
	/// <summary>
	/// Можно ли игроку использовать карту
	/// </summary>
	/// <returns>'true' можно использовать, 'false' использовать нельзя</returns>
	bool CanUse(CardPlayer player);

	/// <summary>
	/// Можно ли игроку нести шмотку (в рюкзаке)
	/// </summary>
	/// <returns>'true' можно использовать, 'false' использовать нельзя</returns>
	bool CanCarry(CardPlayer player);
}
