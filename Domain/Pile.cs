using System;
using Domain.Enums;

namespace Domain;

public class Pile
{
    public PileDirection Direction { get; set; }
    public int TopCardValue { get; set; }
    public bool CanPlay(Card card)
    {
        if (Direction == PileDirection.Ascending && card.ValueCard > TopCardValue)
        {
            return true;
        }
        if (Direction == PileDirection.Descending && card.ValueCard < TopCardValue)
        {
            return true;
        }

        return false;
    }

    public void Play(Card card)
    {
        TopCardValue = card.ValueCard;
    }
}
