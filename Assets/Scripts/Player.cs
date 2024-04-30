using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;

    public static int bonusHP = 0;

    private void Awake()
    {
        hpText.text = $"HP: {3 + bonusHP}";
    }
}
