using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum NodeState
{
    Obtained, // Vert
    Accessible, // Jaune
    Unaccessible // Rouge
}

public class NodeButton : MonoBehaviour
{
    [SerializeField] NodeButton parentNode;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text valueText;
    [SerializeField] int bonusHP = 1;

    LineRenderer lineRenderer;
    NodeState currentState = NodeState.Unaccessible;
    List<NodeButton> children = new List<NodeButton>();

    private void Awake()
    {
        valueText.text = $"+{bonusHP} HP";
        if (parentNode != null)
        {
            parentNode.children.Add(this);
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, parentNode.transform.position);
        }
    }

    private void Start()
    {
        if (parentNode == null)
        {
            // On est à la racine
            SetState(NodeState.Accessible);
        }
    }

    private void SetState(NodeState nodeState)
    {
        currentState = nodeState;
        switch (currentState)
        {
            case NodeState.Obtained:
                Player.bonusHP += bonusHP;

                spriteRenderer.color = Color.green;
                foreach (var child in children)
                    child.SetState(NodeState.Accessible);
                break;
            case NodeState.Accessible:
                spriteRenderer.color = new Color(1, 0.75f, 0);
                foreach (var child in children)
                    child.SetState(NodeState.Unaccessible);
                break;
            case NodeState.Unaccessible:
                spriteRenderer.color = Color.red;
                foreach (var child in children)
                    child.SetState(NodeState.Unaccessible);
                break;
        }
    }

    private void OnMouseDown()
    {
        if (currentState == NodeState.Accessible)
        {
            SetState(NodeState.Obtained);
        }
    }
}
