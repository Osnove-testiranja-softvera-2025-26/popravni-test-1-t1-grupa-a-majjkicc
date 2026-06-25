using NUnit.Framework;
using OTS2026_PT1_GrupaA.Exceptions;
using OTS2026_PT1_GrupaA.Models;
using System;
using System.Collections;
using System.IO;

namespace OTS2026_PT1_GrupaA.Test
{
    public class GameTest
    {
        private Game game;

        [SetUp]
        public void Setup()
        {
            game = new Game(
                new Position(10, 10),
                new Position(5, 5)
            );
        }

        [Test]
        public void Game_ValidPlayerAndBoatPosition_DoesNotThrowException()
        {
            Assert.DoesNotThrow((TestDelegate)(() =>
                new Game(new Position(10, 10), new Position(5, 5))));
        }

        [Test]
        public void Game_PlayerNotInLandZone_ThrowsInvalidPlayerPositionException()
        {
            var ex = Assert.Throws<InvalidPlayerPositionException>((TestDelegate)(() =>
                new Game(new Position(25, 10), new Position(5, 5))));

            Assert.That(ex.Message, Is.EqualTo("Player and boat must be in the Land zone!"));
        }

        [Test]
        public void Game_BoatNotInLandZone_ThrowsInvalidPlayerPositionException()
        {
            var ex = Assert.Throws<InvalidPlayerPositionException>((TestDelegate)(() =>
                new Game(new Position(10, 10), new Position(25, 10))));

            Assert.That(ex.Message, Is.EqualTo("Player and boat must be in the Land zone!"));
        }

        [Test]
        public void ValidatePosition_PositionIsNull_ReturnsFalse()
        {
            Assert.That(game.ValidatePosition(null));
        }

        [Test]
        public void ValidatePosition_PositionOutsideMap_ReturnsFalse()
        {
            Assert.That(game.ValidatePosition(new Position(-1, 10)));
        }

        [Test]
        public void ValidatePosition_PositionInInvalidZone_ReturnsFalse()
        {
            Assert.That(game.ValidatePosition(new Position(0, 0)));
        }

        [Test]
        public void ValidatePosition_PositionInLandZone_ReturnsTrue()
        {
            Assert.That(game.ValidatePosition(new Position(10, 10)));
        }

        [Test]
        public void ValidatePosition_PositionInPondWithoutBoat_ReturnsFalse()
        {
            game.Player.HasBoat = false;

            Assert.That(game.ValidatePosition(new Position(25, 10)));
        }

        [Test]
        public void ValidatePosition_PositionInPondWithBoat_ReturnsTrue()
        {
            game.Player.HasBoat = true;

            Assert.That(game.ValidatePosition(new Position(25, 10)));
        }

        [Test]
        public void MovePlayer_ValidMove_ChangesPlayerPosition()
        {
            game.Player.Position = new Position(10, 10);

            game.MovePlayer(Move.Up);

            Assert.That(10, Is.EqualTo(game.Player.Position.X));
            Assert.That(9, Is.EqualTo(game.Player.Position.Y));
        }

        [Test]
        public void MovePlayer_InvalidMove_DoesNotChangePlayerPosition()
        {
            game.Player.Position = new Position(10, 5);

            game.MovePlayer(Move.Up);

            Assert.That(10, Is.EqualTo(game.Player.Position.X));
            Assert.That(5, Is.EqualTo(game.Player.Position.Y));
        }

        [Test]
        public void ResolvePlayerPosition_PlayerOnBait_IncreasesBaitAndEmptiesField()
        {
            game.Player.Position = new Position(10, 10);
            game.Map.Fields[10, 10].Content = FieldContent.Bait;

            game.ResolvePlayerPosition();

            Assert.That(1, Is.EqualTo(game.Player.AmountOfBait));
            Assert.That(FieldContent.Empty, Is.EqualTo(game.Map.Fields[10, 10].Content));
        }

        [Test]
        public void ResolvePlayerPosition_PlayerOnBoat_SetsHasBoatAndEmptiesField()
        {
            game.Player.Position = new Position(10, 10);
            game.Map.Fields[10, 10].Content = FieldContent.Boat;

            game.ResolvePlayerPosition();

            Assert.That(game.Player.HasBoat);
            Assert.That(FieldContent.Empty, Is.EqualTo(game.Map.Fields[10, 10].Content));
        }

        [Test]
        public void ResolvePlayerPosition_PlayerOnFishWithBait_CatchesFish()
        {
            game.Player.HasBoat = true;
            game.Player.AmountOfBait = 3;
            game.Player.AmountOfFish = 0;
            game.Player.Position = new Position(25, 10);
            game.Map.Fields[25, 10].Content = FieldContent.Fish;

            game.ResolvePlayerPosition();

            Assert.That(2, Is.EqualTo(game.Player.AmountOfBait));
            Assert.That(1, Is.EqualTo(game.Player.AmountOfFish));
            Assert.That(FieldContent.Empty, Is.EqualTo(game.Map.Fields[25, 10].Content));
        }

        [Test]
        public void ResolvePlayerPosition_PlayerOnFishWithoutBait_DoesNotCatchFish()
        {
            game.Player.HasBoat = true;
            game.Player.AmountOfBait = 0;
            game.Player.AmountOfFish = 0;
            game.Player.Position = new Position(25, 10);
            game.Map.Fields[25, 10].Content = FieldContent.Fish;

            game.ResolvePlayerPosition();

            Assert.That(0, Is.EqualTo(game.Player.AmountOfBait));
            Assert.That(0, Is.EqualTo(game.Player.AmountOfFish));
            Assert.That(FieldContent.Empty, Is.EqualTo(game.Map.Fields[25, 10].Content));
        }

        [TestCaseSource(typeof(Parser), nameof(Parser.GetCalculateIncomeData))]
        public void CalculateIncome_ReturnsExpectedScore(
            int fish,
            int bait,
            bool hasBoat,
            Game.Score expected)
        {
            game.Player.AmountOfFish = fish;
            game.Player.AmountOfBait = bait;
            game.Player.HasBoat = hasBoat;

            Game.Score actual = game.CalculateIncome();

            
        }
    

private static Game.Score ParseScore(string value)
        {
            switch (value.Trim())
            {
                case "Bad":
                    return Game.Score.Bad;

                case "Average":
                    return Game.Score.Average;

                case "Good":
                    return Game.Score.Good;

                default:
                    throw new ArgumentException("Unknown score: " + value);
            }
        }
    }
}
