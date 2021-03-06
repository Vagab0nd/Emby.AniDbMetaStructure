﻿using Emby.AniDbMetaStructure.Process.Sources;
using Emby.AniDbMetaStructure.SourceDataLoaders;
using LanguageExt;

namespace Emby.AniDbMetaStructure.Process
{
    /// <summary>
    ///     A source used as the source of additional data (data related to but not directly about the media item being
    ///     processed)
    ///     E.g. The series of an episode
    /// </summary>
    internal class AdditionalSource : ISource
    {
        private readonly ISource sourceImplementation;

        public AdditionalSource(ISource sourceImplementation)
        {
            this.sourceImplementation = sourceImplementation;
            this.Name = new SourceName(this.sourceImplementation.Name + "_Additional");
        }

        public SourceName Name { get; }

        public Either<ProcessFailedResult, IEmbySourceDataLoader> GetEmbySourceDataLoader(IMediaItemType mediaItemType)
        {
            return this.sourceImplementation.GetEmbySourceDataLoader(mediaItemType);
        }

        public bool ShouldUsePlaceholderSourceData(IMediaItemType mediaItemType)
        {
            return false;
        }
    }
}