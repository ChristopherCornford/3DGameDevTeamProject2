using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
	shortSword, 
	axe,
	smallMeal,
	bigMeal
}
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Items : ScriptableObject{

	public new string name;
	public ItemType itemType;
	[Space]
	public GameObject prefab;

}
