﻿using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt.UnsafeValueAccess;
using MediaBrowser.Model.Logging;
using MediaBrowser.Plugins.AniMetadata.JsonApi;
using MediaBrowser.Plugins.AniMetadata.Tests.TestHelpers;
using MediaBrowser.Plugins.AniMetadata.TvDb;
using NUnit.Framework;

namespace MediaBrowser.Plugins.AniMetadata.Tests.IntegrationTests
{
    using Infrastructure;

    [TestFixture]
    [Explicit]
    internal class TvDbTokenIntegrationTests
    {
        [SetUp]
        public void Setup()
        {
            this.logManager = new ConsoleLogManager();
        }

        private ILogManager logManager;

        [Test]
        public async Task GetToken_ExistingToken_DoesNotRequestNewToken()
        {
            var tvDbConnection = new JsonConnection(new TestHttpClient(), new JsonSerialiser(), this.logManager);

            var token = new TvDbToken(tvDbConnection, Secrets.TvDbApiKey, this.logManager);

            var token1 = await token.GetTokenAsync();

            var token2 = await token.GetTokenAsync();

            token2.IsSome.Should().BeTrue();
            token2.ValueUnsafe().Should().Be(token1.ValueUnsafe());
        }

        [Test]
        public async Task GetToken_FailedRequest_ReturnsNone()
        {
            var tvDbConnection = new JsonConnection(new TestHttpClient(), new JsonSerialiser(), this.logManager);

            var token = new TvDbToken(tvDbConnection, "NotValid", this.logManager);

            var returnedToken = await token.GetTokenAsync();

            returnedToken.IsSome.Should().BeFalse();
        }

        [Test]
        public async Task GetToken_NoExistingToken_GetsNewToken()
        {
            var tvDbConnection = new JsonConnection(new TestHttpClient(), new JsonSerialiser(), this.logManager);

            var token = new TvDbToken(tvDbConnection, Secrets.TvDbApiKey, this.logManager);

            var returnedToken = await token.GetTokenAsync();

            returnedToken.IsSome.Should().BeTrue();
            returnedToken.ValueUnsafe().Should().NotBeNullOrWhiteSpace();
        }
    }
}