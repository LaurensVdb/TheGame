using System;

namespace Domain;

public class Deck
{
    public List<Card> Cards { get; set; } = new List<Card>();
    public bool IsEmpty => Cards.Count == 0;

    public Deck()
    {
        for (int i = 2; i < 100; i++)
        {
            Cards.Add(new Card()
            {
                ValueCard = i
            });
        }
    }

    public void Shuffle()
    {
        Random r = new Random();

        for (int i = 0; i < Cards.Count; i++)
        {
            int randomIndex = r.Next(i, Cards.Count);
            var temp = Cards[i];
            Cards[i] = Cards[randomIndex];
            Cards[randomIndex] = temp;
        }

    }

    public Card? Draw()
    {
        var card = Cards.FirstOrDefault();
        if (card != null)
        {
            Cards.Remove(card);
        }
        return card;
    }
}
