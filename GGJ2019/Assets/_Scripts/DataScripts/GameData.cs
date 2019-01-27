using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Data", menuName="Data/GameData")]
public class GameData : ScriptableObject {

	[Tooltip("Start position")]
	public Vector3 StartPosition;

	[Tooltip("Start lives")]
	public int StartLives;

}
