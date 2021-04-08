using System;
using Bogus;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;
using MusicGames.Domain.AggregatesModels.MusicAggregate;
using Xunit;
using Xunit.Abstractions;

namespace MusicGames.Domain.Test
{
    public class GamePlayListTest
    {
        private readonly ITestOutputHelper _output;
        private readonly Faker _randomFluent;

        public GamePlayListTest(ITestOutputHelper output)
        {
            _output = output;
            _randomFluent = new Faker
            {
                Random = new Randomizer(1080)
            };
        }

        [Fact]
        public void Validate_GamePlayList_ReturnsTrue()
        {
            var playlist = new Playlist<GameTrack>();
            for (var count = 0; count < 10; count++)
            {
                var fakeSong = new Song
                {
                    Title = _randomFluent.Hacker.Phrase(),
                    Album = _randomFluent.Hacker.Phrase(),
                    Composer = _randomFluent.Person.FullName
                };

                var fakeGameId = _randomFluent.Random.Int(0, 100);

                var fakeMode = new DifficultyMode();

                playlist.Songs.Add(new Ez2OnGameTrack(fakeSong, fakeGameId, fakeMode));
            }

            playlist.Name = _randomFluent.Hacker.Phrase();
            playlist.DateTimeCreated = _randomFluent.Date.Past(10, DateTime.Now);
            playlist.DateTimeModified = _randomFluent.Date.Future(10, DateTime.Now);

            var validator = new GameTrackPlaylistValidator();
            var validationResult = validator.Validate(playlist);
            if (!validationResult.IsValid)
                foreach (var error in validationResult.Errors)
                    _output.WriteLine(error.ToString());
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Assign_DefaultDateTimeCreated_To_GamePlayList_ReturnsFalse()
        {
            var playlist = new Playlist<GameTrack>();
            for (var count = 0; count < 10; count++)
            {
                var fakeSong = new Song
                {
                    Title = _randomFluent.Hacker.Phrase(),
                    Album = _randomFluent.Hacker.Phrase(),
                    Composer = _randomFluent.Person.FullName
                };

                var fakeGameId = _randomFluent.Random.Int(0, 100);

                var fakeMode = new DifficultyMode();

                playlist.Songs.Add(new Ez2OnGameTrack(fakeSong, fakeGameId, fakeMode));
            }

            playlist.Name = _randomFluent.Hacker.Phrase();
            playlist.DateTimeModified = DateTime.Now;
            var validator = new GameTrackPlaylistValidator();
            var validationResult = validator.Validate(playlist);
            if (!validationResult.IsValid)
                foreach (var error in validationResult.Errors)
                    _output.WriteLine(error.ToString());
            Assert.Contains(validationResult.Errors,
                x => x.ErrorMessage == GameTrackPlaylistValidator.DateTimeCreatedErrorMessage);
        }

        [Fact]
        public void Assign_DefaultDateTimeModified_To_GamePlayList_ReturnsFalse()
        {
            var playlist = new Playlist<GameTrack>();
            for (var count = 0; count < 10; count++)
            {
                var fakeSong = new Song
                {
                    Title = _randomFluent.Hacker.Phrase(),
                    Album = _randomFluent.Hacker.Phrase(),
                    Composer = _randomFluent.Person.FullName
                };

                var fakeGameId = _randomFluent.Random.Int(0, 100);

                var fakeMode = new DifficultyMode();

                playlist.Songs.Add(new Ez2OnGameTrack(fakeSong, fakeGameId, fakeMode));
            }

            playlist.Name = _randomFluent.Hacker.Phrase();
            playlist.DateTimeCreated = DateTime.Now;
            var validator = new GameTrackPlaylistValidator();
            var validationResult = validator.Validate(playlist);
            if (!validationResult.IsValid)
                foreach (var error in validationResult.Errors)
                    _output.WriteLine(error.ToString());
            Assert.Contains(validationResult.Errors,
                x => x.ErrorMessage == GameTrackPlaylistValidator.DateTimeModifiedErrorMessage);
        }
    }
}