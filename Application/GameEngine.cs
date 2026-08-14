using System;
using Domain;
using Domain.Enums;

namespace Application;

public interface IGameEngine
{
    void ApplyMove(GameState gameState, Move move);
    bool CheckGameOver(GameState state);
    void EndTurn(GameState state);
    bool HasValidMove(GameState state, Player player);
    void StartTurn(GameState state);
    MoveResult ValidateMove(GameState gameState, Move move);
}

public class GameEngine : IGameEngine
{
    public MoveResult ValidateMove(GameState gameState, Move move)
    {
        return MoveResult.Valid;
    }

    public void ApplyMove(GameState gameState, Move move)
    {

    }
    public bool HasValidMove(GameState state, Player player)
    {
        return true;
    }

    public void StartTurn(GameState state) { }

    public void EndTurn(GameState state) { }

    public bool CheckGameOver(GameState state)
    {
        return false;
    }
}
