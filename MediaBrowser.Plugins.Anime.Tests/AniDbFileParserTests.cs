﻿using System;
using System.IO;
using FluentAssertions;
using MediaBrowser.Plugins.Anime.AniDb;
using MediaBrowser.Plugins.Anime.AniDb.SeriesData;
using NUnit.Framework;

namespace MediaBrowser.Plugins.Anime.Tests
{
    [TestFixture]
    public class AniDbFileParserTests
    {
        [Test]
        public void ParseSeriesFile_ValidXml_ReturnsDeserialised()
        {
            var fileContent = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\TestData\anidb\series.xml");

            var aniDbFileParser = new AniDbFileParser();

            var series = aniDbFileParser.ParseSeriesXml(fileContent);

            series.Id.Should().Be(1);
            series.Restricted.Should().BeFalse();

            series.EpisodeCount.Should().Be(13);
            series.StartDate.Should().Be(new DateTime(1999, 1, 3));
            series.EndDate.Should().Be(new DateTime(1999, 3, 28));

            series.Titles.ShouldBeEquivalentTo(new[]
            {
                new SeriesTitle
                {
                    Language = "x-jat",
                    Type = "main",
                    Title = "Seikai no Monshou"
                },
                new SeriesTitle
                {
                    Language = "cs",
                    Type = "synonym",
                    Title = "Hvězdný erb"
                },
                new SeriesTitle
                {
                    Language = "zh-Hans",
                    Type = "synonym",
                    Title = "星界之纹章"
                },
                new SeriesTitle
                {
                    Language = "en",
                    Type = "short",
                    Title = "CotS"
                },
                new SeriesTitle
                {
                    Language = "x-jat",
                    Type = "short",
                    Title = "SnM"
                },
                new SeriesTitle
                {
                    Language = "ja",
                    Type = "official",
                    Title = "星界の紋章"
                },
                new SeriesTitle
                {
                    Language = "en",
                    Type = "official",
                    Title = "Crest of the Stars"
                },
                new SeriesTitle
                {
                    Language = "fr",
                    Type = "official",
                    Title = "Crest of the Stars"
                },
                new SeriesTitle
                {
                    Language = "pl",
                    Type = "official",
                    Title = "Crest of the Stars"
                }
            });

            series.RelatedSeries.ShouldBeEquivalentTo(new[]
            {
                new RelatedSeries
                {
                    Id = 4,
                    Type = "Sequel",
                    Title = "Seikai no Senki"
                },
                new RelatedSeries
                {
                    Id = 6,
                    Type = "Prequel",
                    Title = "Seikai no Danshou: Tanjou"
                },
                new RelatedSeries
                {
                    Id = 1623,
                    Type = "Summary",
                    Title = "Seikai no Monshou Tokubetsu Hen"
                }
            });

            series.SimilarSeries.ShouldBeEquivalentTo(new[]
            {
                new SimilarSeries
                {
                    Id = 584,
                    Approval = 70,
                    Total = 84,
                    Title = "Ginga Eiyuu Densetsu"
                },
                new SimilarSeries
                {
                    Id = 2745,
                    Approval = 51,
                    Total = 61,
                    Title = "Starship Operators"
                },
                new SimilarSeries
                {
                    Id = 6005,
                    Approval = 34,
                    Total = 49,
                    Title = "Tytania"
                },
                new SimilarSeries
                {
                    Id = 192,
                    Approval = 18,
                    Total = 40,
                    Title = "Mugen no Ryvius"
                },
                new SimilarSeries
                {
                    Id = 630,
                    Approval = 13,
                    Total = 27,
                    Title = "Uchuu no Stellvia"
                },
                new SimilarSeries
                {
                    Id = 5406,
                    Approval = 3,
                    Total = 15,
                    Title = "Ookami to Koushinryou"
                },
                new SimilarSeries
                {
                    Id = 18,
                    Approval = 2,
                    Total = 10,
                    Title = "Musekinin Kanchou Tylor"
                }
            });

            series.Url.Should().Be("http://www.sunrise-inc.co.jp/seikai/");

            series.Creators.ShouldBeEquivalentTo(new[]
            {
                new Creator
                {
                    Id = 4303,
                    Type = "Music",
                    Name = "Hattori Katsuhisa"
                },
                new Creator
                {
                    Id = 4234,
                    Type = "Direction",
                    Name = "Nagaoka Yasuchika"
                },
                new Creator
                {
                    Id = 4516,
                    Type = "Character Design",
                    Name = "Watabe Keisuke"
                },
                new Creator
                {
                    Id = 8924,
                    Type = "Series Composition",
                    Name = "Yoshinaga Aya"
                },
                new Creator
                {
                    Id = 4495,
                    Type = "Original Work",
                    Name = "Morioka Hiroyuki"
                }
            });

            series.Description.Should()
                .Be(
                    "* Based on the sci-fi novel series by http://anidb.net/cr4495 [Morioka Hiroyuki].\nhttp://anidb.net/ch4081 [Linn Jinto]`s life changes forever when the http://anidb.net/ch7514 [Humankind Empire Abh] takes over his home planet of Martine without firing a single shot. He is soon sent off to study the http://anidb.net/t2324 [Abh] language and culture and to prepare himself for his future as a nobleman — a future he never dreamed of, asked for, or even wanted.\nNow, Jinto is entering the next phase of his training, and he is about to meet the first Abh in his life, the lovely http://anidb.net/ch28 [Lafiel]. However, Jinto is about to learn that there is more to her than meets the eye, and together they will have to fight for their very lives.");

            series.Ratings.ShouldBeEquivalentTo(new Rating[]
            {
                new PermanentRating
                {
                    Count = 4303,
                    Value = 8.17m
                },
                new TemporaryRating
                {
                    Count = 4333,
                    Value = 8.26m
                },
                new ReviewRating
                {
                    Count = 12,
                    Value = 8.70m
                }
            });

            series.Tags.Length.Should().Be(51);
            series.Tags[0].ShouldBeEquivalentTo(new Tag
            {
                Id = 36,
                ParentId = 2607,
                Weight = 300,
                LocalSpoiler = false,
                GlobalSpoiler = false,
                Verified = true,
                LastUpdated = new DateTime(2017, 4, 17),
                Name = "military",
                Description =
                    "The military, also known as the armed forces, are forces authorized and legally entitled to use deadly force so as to support the interests of the state and its citizens. The task of the military is usually defined as defence of the state and its citizens and the prosecution of war against foreign powers. The military may also have additional functions within a society, including construction, emergency services, social ceremonies, and guarding critical areas.\nSource: Wikipedia"
            });

            series.Episodes.Length.Should().Be(16);
            series.Episodes[0].ShouldBeEquivalentTo(new Episode
            {
                Id = 1,
                LastUpdated = new DateTime(2011, 7, 1),
                EpisodeNumber = new EpisodeNumber
                {
                    Type = 1,
                    Number = "1"
                },
                TotalMinutes = 25,
                AirDate = new DateTime(1999, 1, 3),
                Rating = new EpisodeRating
                {
                    VoteCount = 28,
                    Rating = 3.16m
                },
                Titles = new[]
                {
                    new EpisodeTitle
                    {
                        Language = "ja",
                        Title = "侵略"
                    },
                    new EpisodeTitle
                    {
                        Language = "en",
                        Title = "Invasion"
                    },
                    new EpisodeTitle
                    {
                        Language = "fr",
                        Title = "Invasion"
                    },
                    new EpisodeTitle
                    {
                        Language = "x-jat",
                        Title = "Shinryaku"
                    }
                }
            });
        }
    }
}