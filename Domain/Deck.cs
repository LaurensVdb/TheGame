using System;

namespace Domain;

public class Deck
{
    public List<Card> Cards { get; set; } = new List<Card>();
    public bool IsEmpty => Cards.Count == 0;
    public void Shuffle()
    {

    }

    public Card Draw()
    {
        return new Card();
    }

}
