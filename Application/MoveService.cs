using System;
using Domain;
using Domain.Enums;

namespace Application;

public interface IMoveService
{
    List<Move> GetAllValidMoves(GameState state, Player player);
    bool IsReverseMove(Pile pile, Card card);
}

public class MoveService : IMoveService
{
    public List<Move> GetAllValidMoves(GameState state, Player player)
    {
        return new List<Move>();
    }

    public bool IsReverseMove(Pile pile, Card card)
    {
        return false;
    }
}
