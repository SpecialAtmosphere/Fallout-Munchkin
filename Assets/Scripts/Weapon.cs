using UnityEngine;

public class Weapon : MonoBehaviour
{
	public bool InTwoArms;
	public WeaponTypes WeaponType;
	public bool FireDamage;

	public enum WeaponTypes
	{
		Steel,
		Light,
		Heavy,
		Energy,
	}
}
