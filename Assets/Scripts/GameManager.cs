using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameTilePrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] TMP_Text goldText;
    GameTile[,] gameTiles;
    private GameTile spawnTile;
    const int ColCount = 20;
    const int RowCount = 10;

    [SerializeField] public int gold = 10;

    public GameTile TargetTile { get; internal set; }
    List<GameTile> pathToGoal = new List<GameTile>();

    private void Awake()
    {
        gameTiles = new GameTile[ColCount, RowCount];

        for (int x = 0; x < ColCount; x++)
        {
            for (int y = 0; y < RowCount; y++)
            {
                var spawnPosition = new Vector3(x, y, 0);
                var tile = Instantiate(gameTilePrefab, spawnPosition, Quaternion.identity);
                gameTiles[x, y] = tile.GetComponent<GameTile>();
                gameTiles[x, y].GM = this;
                gameTiles[x, y].X = x;
                gameTiles[x, y].Y = y;

                if ((x + y) % 2 == 0)
                {
                    gameTiles[x, y].TurnGrey();
                }
            }
        }

        spawnTile = gameTiles[1, 7];
        spawnTile.SetEnemySpawn();
        TargetTile = gameTiles[16, 3];
        for (int y = 2; y <= 9; y++)
        {
            gameTiles[5, y].SetWall();
        }

        for (int y = 0; y <= 7; y++)
        {
            gameTiles[10, y].SetWall();
        }      
    }
    private void Start()
    {
        foreach (var t in gameTiles)
        {
            t.SetPath(false);
        }

        var path = Pathfinding(spawnTile, TargetTile);
        var tile = TargetTile;

        while (tile != null)
        {
            pathToGoal.Add(tile);
            tile.SetPath(true);
            tile = path[tile];
        }
        StartCoroutine(SpawnEnemyCoroutine());
    }

    private void Update()
    {
        goldText.text = $"Gold:{gold}";
    }

    private Dictionary<GameTile, GameTile> Pathfinding(GameTile sourceTile, GameTile targetTile)
    {
        var dist = new Dictionary<GameTile, int>();

        var prev = new Dictionary<GameTile, GameTile>();

        var Q = new List<GameTile>();

        foreach (var v in gameTiles)
        {
            dist.Add(v, 9999);

            prev.Add(v, null);

            Q.Add(v);
        }

        dist[sourceTile] = 0;

        while (Q.Count > 0)
        {
            GameTile u = null;
            int minDistance = int.MaxValue;

            foreach (var v in Q)
            {
                if (dist[v] < minDistance)
                {
                    minDistance = dist[v];
                    u = v;
                }
            }

            Q.Remove(u);
            
            foreach (var v in FindNeighbor(u))
            {
                if (!Q.Contains(v) || v.IsBlocked)
                {
                    continue;
                }

                int alt = dist[u] + 1;

                if (alt < dist[v])
                {
                    dist[v] = alt;

                    prev[v] = u;
                }
            }
        }

        return prev;
    }

    private List<GameTile> FindNeighbor(GameTile u)
    {
        var result = new List<GameTile>();

        if (u.X - 1 >= 0)
            result.Add(gameTiles[u.X - 1, u.Y]);
        if (u.X + 1 < ColCount)
            result.Add(gameTiles[u.X + 1, u.Y]);
        if (u.Y - 1 >= 0)
            result.Add(gameTiles[u.X, u.Y - 1]);
        if (u.Y + 1 < RowCount)
            result.Add(gameTiles[u.X, u.Y + 1]);

        return result;
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        yield return new WaitForSeconds(5f);
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.6f);
                var enemy = Instantiate(enemyPrefab, spawnTile.transform.position, Quaternion.identity).GetComponent<Enemy>();
                enemy.GM = this;
                enemy.SetPath(pathToGoal);
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
