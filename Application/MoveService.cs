using System;
using System.IO.Pipes;
using Domain;
using Domain.Enums;
using Microsoft.VisualBasic;

namespace Application;

public interface IMoveService
{
    List<Move> GetAllValidMoves(GameState state);

    ValidationResult ValidateMove(Move move);
}

public class MoveService : IMoveService
{
    public List<Move> GetAllValidMoves(GameState state)
    {
        var piles = new[]
        {
        state.AscendingPile1,
        state.DescendingPile1,
        state.AscendingPile2,
        state.DescendingPile2
    };

        return state.CurrentPlayer.Hand
            .SelectMany(card => piles
            .Select(pile => new Move { Player = state.CurrentPlayer, Card = card, TargetPile = pile }))
            .Where(move => ValidateMove(move).IsValid)
            .ToList();
    }

    public ValidationResult ValidateMove(Move move)
    {
        if (!move.TargetPile.CanPlay(move.Card) && !IsReverseMove(move.TargetPile, move.Card))
        {
            return new ValidationResult(false, "Invalid move.");
        }
        return new ValidationResult(true);
    }

    private bool IsReverseMove(Pile pile, Card card)
    {
        if (pile.Direction == PileDirection.Ascending && card.ValueCard == pile.TopCardValue - 10)
        {
            return true;
        }
        if (pile.Direction == PileDirection.Descending && card.ValueCard == pile.TopCardValue + 10)
        {
            return true;
        }
        return false;
    }
}
