using System;

namespace Domain;

public class Player
{

    public string Name { get; set; }
    public List<Card> Hand { get; set; } = new List<Card>();
    public bool IsAi { get; set; }

    public void DrawCardFromDeck(Deck deck, int numberOfCards = 1)
    {

    }
}
