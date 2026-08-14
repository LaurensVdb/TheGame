using System;
using Domain.Enums;

namespace Domain;

public class Pile
{
    public PileDirection Direction { get; set; }
    public int TopCardValue { get; set; }
    public bool CanPlay(Card card)
    {
        return true;
    }

    public void Play(Card card)
    {

    }
}
