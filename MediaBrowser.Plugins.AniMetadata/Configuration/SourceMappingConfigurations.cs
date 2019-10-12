﻿using System.Collections;
using System.Collections.Generic;
using MediaBrowser.Plugins.AniMetadata.AniDb;
using MediaBrowser.Plugins.AniMetadata.AniList;
using MediaBrowser.Plugins.AniMetadata.TvDb;

namespace MediaBrowser.Plugins.AniMetadata.Configuration
{
    internal class SourceMappingConfigurations : IEnumerable<ISourceMappingConfiguration>
    {
        private readonly IEnumerable<ISourceMappingConfiguration> sourceMappingConfigurations;

        public SourceMappingConfigurations(AniDbSourceMappingConfiguration aniDbSourceMappingConfiguration,
            TvDbSourceMappingConfiguration tvDbSourceMappingConfiguration,
            AniListSourceMappingConfiguration aniListSourceMappingConfiguration)
        {
            this.sourceMappingConfigurations =
                new ISourceMappingConfiguration[]
                {
                    aniDbSourceMappingConfiguration,
                    tvDbSourceMappingConfiguration,
                    aniListSourceMappingConfiguration
                };
        }

        public IEnumerator<ISourceMappingConfiguration> GetEnumerator()
        {
            return this.sourceMappingConfigurations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.sourceMappingConfigurations).GetEnumerator();
        }
    }
}