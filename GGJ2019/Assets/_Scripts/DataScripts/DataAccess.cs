using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class DataAccess {

	public EngineMoveData EngineMoveData { get { return engineMoveData; } }
	EngineMoveData engineMoveData;

	public GameData GameData { get { return gameData; } }
	GameData gameData;

	public int Lives { get { return lives; } }
	int lives;

	public void Load() {
		//engineMoveData = AssetDatabase.LoadAssetAtPath("Assets/Data/Move/EngineMoveData.asset", typeof(EngineMoveData)) as EngineMoveData;
		//gameData = AssetDatabase.LoadAssetAtPath("Assets/Data/GameParameters.asset", typeof(GameData)) as GameData;
		engineMoveData = Resources.Load<EngineMoveData>("Move/EngineMoveData");
		gameData = Resources.Load<GameData>("GameParameters");
		lives = gameData.StartLives;
	}

	public void RemoveLive() {
		lives--;
	}

	public void ResetLives() {
		lives = gameData.StartLives;
	}

	// Singleton
	private DataAccess() {
		Load();
	}

	private static DataAccess instance;
	public static DataAccess Instance {
		get {
			if (instance == null) instance = new DataAccess();
			return instance;
		}
	}

}
