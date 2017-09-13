﻿using FluentAssertions;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Plugins.AniMetadata.MetadataMapping;
using NUnit.Framework;

namespace MediaBrowser.Plugins.AniMetadata.Tests
{
    [TestFixture]
    public class PropertyMappingTests
    {
        private class Source
        {
            public string SourceValue => "Source";
        }

        private class Target : BaseItem
        {
            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
            public string TargetValue { get; set; } = "Target";
        }

        [Test]
        public void Map_CopiesValueFromSourceToTarget()
        {
            var propertyMapping =
                new PropertyMapping<Source, Target, string, string>(s => s.SourceValue, t => t.TargetValue);

            var source = new Source();
            var target = new Target();

            propertyMapping.Map(source, target);

            target.TargetValue.Should().Be("Source");
        }

        [Test]
        public void SourcePropertyName_ReturnsSelectedSourceProperty()
        {
            var propertyMapping =
                new PropertyMapping<Source, Target, string, string>(s => s.SourceValue, t => t.TargetValue);

            propertyMapping.SourcePropertyName.Should().Be("SourceValue");
        }

        [Test]
        public void TargetPropertyName_ReturnsSelectedTargetProperty()
        {
            var propertyMapping =
                new PropertyMapping<Source, Target, string, string>(s => s.SourceValue, t => t.TargetValue);

            propertyMapping.TargetPropertyName.Should().Be("TargetValue");
        }
    }
}