using System;
using Domain;

namespace Application;

public interface ICardService
{
    List<Card> GenerateFullDeck();
    List<Card> Shuffle(List<Card> cards);
}

public class CardService : ICardService
{
    public List<Card> GenerateFullDeck()
    {
        return new List<Card>();
    }

    public List<Card> Shuffle(List<Card> cards)
    {
        return new List<Card>();
    }
}
