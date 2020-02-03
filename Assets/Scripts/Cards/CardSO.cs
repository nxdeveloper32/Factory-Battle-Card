using UnityEngine;
[CreateAssetMenu(menuName = "CardGame/Card")]
public class CardSO : ScriptableObject
{
    public string cardName;
    public string CardDescription;
    public string SpecialPower;
    public CardClass Cardclass;
    public CardType CardType;
    public int Point;
    public Sprite CardArt;
    public Sprite CardBackgroundArt;
    public Sprite[] cardTypeArt;
}

