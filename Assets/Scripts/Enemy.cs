using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject visual;
    public GameManager GM {  get; set; }
    public static HashSet<Enemy> allEnemies = new HashSet<Enemy>();
    private Stack<GameTile> path = new Stack<GameTile>();
    int hp = 5;

    private void Awake()
    {
        allEnemies.Add(this);
    }

    internal void SetPath(List<GameTile> pathToGoal)
    {
        path.Clear();
        foreach (GameTile tile in pathToGoal)
        {
            path.Push(tile);
        }
    }

    void Update()
    {
        if (path.Count > 0)
        {
            Vector3 destPos = path.Peek().transform.position;
            transform.position = Vector3.MoveTowards(transform.position, destPos, 2 * Time.deltaTime);

            if (Vector3.Distance(transform.position, destPos) < 0.01f)
            {
                path.Pop();
            }
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        allEnemies.Remove(this);
        Destroy(gameObject);
    }

    internal void Attack()
    {
        if (--hp <= 0)
        {
            GM.gold++;
            Die();
        }
        else
        {
            visual.transform.localScale *= 0.9f;
        }
    }
}
