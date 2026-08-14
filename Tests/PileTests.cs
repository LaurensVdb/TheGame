using System;
using Domain;
using Domain.Enums;

namespace Tests;

public class PileTests
{
    [Fact]
    public void CanPlayCardOnPileAscendingCanPlay()
    {
        var pile = new Pile();
        pile.Direction = PileDirection.Ascending;
        pile.TopCardValue = 3;

        var card = new Card { ValueCard = 5 };

        var canPlay = pile.CanPlay(card);

        Assert.True(canPlay);
    }

    [Fact]
    public void CanPlayCardOnPileAscendingCannotPlay()
    {
        var pile = new Pile();
        pile.Direction = PileDirection.Ascending;
        pile.TopCardValue = 3;

        var card = new Card { ValueCard = 2 };

        var canPlay = pile.CanPlay(card);

        Assert.False(canPlay);
    }

    [Fact]
    public void CanPlayCardOnPileDescendingCanPlay()
    {
        var pile = new Pile();
        pile.Direction = PileDirection.Descending;
        pile.TopCardValue = 7;

        var card = new Card { ValueCard = 4 };

        var canPlay = pile.CanPlay(card);

        Assert.True(canPlay);
    }

    [Fact]
    public void CanPlayCardOnPileDescendingCannotPlay()
    {
        var pile = new Pile();
        pile.Direction = PileDirection.Descending;
        pile.TopCardValue = 7;

        var card = new Card { ValueCard = 8 };

        var canPlay = pile.CanPlay(card);

        Assert.False(canPlay);
    }

    [Fact]
    public void PlayCardOnPileUpdatesTopCardValue()
    {
        var pile = new Pile();
        pile.Direction = PileDirection.Ascending;
        pile.TopCardValue = 3;

        var card = new Card { ValueCard = 5 };

        pile.Play(card);

        Assert.Equal(5, pile.TopCardValue);
    }
}
