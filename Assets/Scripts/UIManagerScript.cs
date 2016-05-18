using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{

	public Animator invBtn;
	public Animator bagBtn;

	void OnEnable()
	{
		Player.OnClassChanged += ChangeClassText;

	}

	void OnDisable()
	{
		Player.OnClassChanged -= ChangeClassText;
	}

	/// <summary>
	/// Открываем инвентарь
	/// </summary>
	public void OpenInventory()
	{
		invBtn.enabled = true;
		if (!invBtn.GetBool("isHidden"))
			invBtn.SetBool("isHidden", true);
		else if (invBtn.GetBool("isHidden"))
			invBtn.SetBool("isHidden", false);
	}

	/// <summary>
	/// Закрываем инвентарь
	/// </summary>
	public void CloseInventory()
	{
		invBtn.SetBool("isHidden", false);
	}

	/// <summary>
	/// Открываем сумку
	/// </summary>
	public void OpenBag()
	{
		bagBtn.enabled = true;
		if (!bagBtn.GetBool("isHidden"))
			bagBtn.SetBool("isHidden", true);
		else if (bagBtn.GetBool("isHidden"))
			bagBtn.SetBool("isHidden", false);
	}

	/// <summary>
	/// Закрываем сумку
	/// </summary>
	public void CloseBag()
	{
		bagBtn.SetBool("isHidden", false);
	}

	/// <summary>
	/// Изменить заголовок у класса
	/// </summary>
	/// <param name="card"></param>
	public void ChangeClassText(Card card)
	{
		var player = GetComponentInParent<Player>();
		var cls_value_text = player.transform.FindDeepChild("player_class").FindChild("value").GetComponent<Text>().text;
		if (player.Classes.Count > 0)
			foreach (var c in player.Classes)
			{
				cls_value_text = c.GetComponent<Door>().Name + "\n";
			}
		else
			cls_value_text = null;
	}
}
