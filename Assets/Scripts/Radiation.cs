using UnityEngine;
using System.Collections;

public class Radiation : MonoBehaviour
{
	public void Acivate(CardPlayer player)
	{
		var card = this.GetComponentInParent<Card>();
		var door = card.GetComponent<Door>();
		var game = GameObject.FindObjectOfType<Game>();
		switch (door.Name)
		{
			case "Back_to_origins":
				foreach (var p in player.Perks)
				{
					player.Perks.Remove(p);
					game.pReset(p);
				}
				foreach (var c in player.Classes)
				{
					player.Classes.Remove(c);
					game.dReset(c);
				}
				game.pReset(card);
				break;
			case "Ghoulification":
				//TODO:Array.Resize(player.Partners, i);
				player.Radiated = true;
				player.Radiation = card;
				player.RadiationDefense++;
				break;
			case "Fatal_outcome":
				player.Killed();
				break;
			case "Radiated":
				player.Level--;
				break;
			case "Mutation":
				//TODO: Изменение двуручного в одноручное
				//Запрет на бронник
				foreach (var i in player.Inventary)
				{
					player.Radiation = card;
					if (i.GetComponent<Staff>().StuffType == StuffTypes.Armor)
					{
						player.Inventary.Remove(i);
						game.tReset(i);
					}
				}
				break;
			case "Retinal_detachment":
				//TODO: Проверка на наличие радиации "3-й глаз" для уменьшения на 1
				player.RunAbility -= 2;
				player.Radiated = true;
				player.Radiation = card;
				break;
			case "Gradual_radiation":
				//TODO: ЭТО ЖОПА! Настроить увеличение штрафа с каждым ходом + смерть + если есть антирадин, -2 лвл 
				player.Radiated = true;
				player.Radiation = card;
				break;
			case "Siamise_twins":
				//TODO: ЭТО ЖОПА Вдвойне!
				player.Radiated = true;
				player.Radiation = card;
				break;
			case "Three-eyed":
				//TODO: Проверка в начале след хода, для уменьшения силы на 5
				player.Radiated = true;
				player.Radiation = card;
				break;
			case "Brain_atrothy":
				//TODO: уменьшение максимума перков на 1 + при выборе уменьшение на 1 перк
				player.Radiated = true;
				player.Radiation = card;
				break;
		}
	}
}
