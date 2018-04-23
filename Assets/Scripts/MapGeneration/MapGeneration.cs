using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour {

	public class Count {
		public int minimum;
		public int maximum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	[Header("How Many Tiles Is The Map?")]
	public int width;
	public int height;

	[Header("Map Variables")]
	public int currentLevel;
	[Range(0, 100)]
	public int fillPercentage;
	public int borderThickness;
	public int minimumWallSize;
	public int minimumRoomSize;

	[Header("Seeding the Random Map")]
	public string seed;
	public bool useRandomSeed;

	[Header("Map Smoothing")]
	public int smoothingPasses;

	[Header("Floor")]
	public GameObject floor;

	[Header("Object Arrays")]
	public GameObject[] items;
	public GameObject[] enemies;
	public GameObject startPoint;
	public GameObject exitPoint;

	public Count itemCount = new Count (5, 10);

	Transform boardManager;
	List<Vector3> gridPositions = new List<Vector3>();

	int[,] map;


	void Start () {
		GenerateMap ();
	}


	public void GenerateMap () {
		map = new int[width, height];
		RandomFillMap ();

		for (int i = 0; i < smoothingPasses; i++) {
			SmoothMap ();
		}

		ProcessMap ();

		int borderSize = borderThickness;
		int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

		for (int x = 0; x < borderedMap.GetLength(0); x++) {
			for (int y = 0; y < borderedMap.GetLength(1); y++) {
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
					borderedMap [x, y] = map [x - borderSize, y - borderSize];
				} else {
					borderedMap [x, y] = 1;
				}
			}
			floor.transform.localScale = new Vector3(width / 10f, 1, height / 10f);
			floor.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (width, height);
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		meshGen.GenerateMesh (borderedMap, 1);

		boardManager = new GameObject ("Board Manager").transform;
		boardManager.transform.position = new Vector3 (-(width / 2), -4.5f, -(height / 2));
		InitializeList ();
		SpawnGameObjectsAtRandom (items, itemCount.minimum, itemCount.maximum);
		int enemyCount = (int)Mathf.Log (currentLevel, 2f);
		SpawnGameObjectsAtRandom (enemies, enemyCount, enemyCount);
	}

	void ProcessMap() {
		List<List<Coord>> wallRegions = GetRegions (1);
		int wallThresholdSize = minimumWallSize;

		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion) {
					map[tile.tileX,tile.tileY] = 0;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions (0);
		int roomThresholdSize = minimumRoomSize;
		
		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					map[tile.tileX,tile.tileY] = 1;
				}
			}
		}
	}

	List<List<Coord>> GetRegions(int tileType) {
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[width,height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (mapFlags[x,y] == 0 && map[x,y] == tileType) {
					List<Coord> newRegion = GetRegionTiles(x,y);
					regions.Add(newRegion);

					foreach (Coord tile in newRegion) {
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}

		return regions;
	}

	List<Coord> GetRegionTiles(int startX, int startY) {
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width,height];
		int tileType = map [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue();
			tiles.Add(tile);

			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
					if (IsInMapRange(x,y) && (y == tile.tileY || x == tile.tileX)) {
						if (mapFlags[x,y] == 0 && map[x,y] == tileType) {
							mapFlags[x,y] = 1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}
			}
		}

		return tiles;
	}

	bool IsInMapRange(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	void RandomFillMap () {
		if (useRandomSeed) {
			seed = (currentLevel * (Random.Range (0, 1000) / 9.24)).ToString();
		}

		System.Random pRNG = new System.Random (seed.GetHashCode ());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1 ){
					map [x, y] = 1;
				} else {
				map [x, y] = (pRNG.Next (0, 100) < fillPercentage) ? 1 : 0;
				}
			}
		}
	}
	void SmoothMap () {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighborWallTiles = GetSurroundingWallCount (x, y);

				if (neighborWallTiles > 4) {
					map [x, y] = 1;
				} else if (neighborWallTiles < 4) {
					map [x, y] = 0;
				}
			}
		}
	}

	int GetSurroundingWallCount (int gridX, int gridY) {
		int wallCount = 0;
		for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++) {
			for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++) {
				if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height) {
					if (neighborX != gridX || neighborY != gridY) {
						wallCount += map [neighborX, neighborY];
					}
				}
				else {
					wallCount++;
				}
			}
		}

		return wallCount;
	}

	struct Coord {
		public int tileX;
		public int tileY;

		public Coord(int x, int y) {
			tileX = x;
			tileY = y;
		}
	}
	void InitializeList() {
		gridPositions.Clear ();

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (IsInMapRange(x, y) && map[x,y] == 0) {
					gridPositions.Add (new Vector3(x, 0, y));
				}
			}
		}
	}
	Vector3 RandomPosition () {

		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}
	void SpawnGameObjectsAtRandom(GameObject[] objectArray, int minimum, int maximum) {
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject toInstantiate = objectArray [Random.Range (0, objectArray.Length)];
			Debug.Log (toInstantiate.name + "at" + randomPosition);
			GameObject newObject = Instantiate (toInstantiate, randomPosition, Quaternion.identity) as GameObject;
			newObject.transform.SetParent (boardManager);
			newObject.transform.position = new Vector3 (0, 0, 0);
			newObject.transform.localPosition = randomPosition;
		}
	}
}
