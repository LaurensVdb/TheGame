using System;

namespace Domain;

public class Move
{
    public Player Player { get; set; }
    public Card Card { get; set; }
    public Pile TargetPile { get; set; }
}