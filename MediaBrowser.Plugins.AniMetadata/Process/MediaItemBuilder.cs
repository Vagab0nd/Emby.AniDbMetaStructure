﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Emby.AniDbMetaStructure.Configuration;
using Emby.AniDbMetaStructure.SourceDataLoaders;
using LanguageExt;
using MediaBrowser.Model.Logging;
using static LanguageExt.Prelude;

namespace Emby.AniDbMetaStructure.Process
{
    internal class MediaItemBuilder : IMediaItemBuilder
    {
        private readonly IPluginConfiguration pluginConfiguration;
        private readonly IEnumerable<ISourceDataLoader> sourceDataLoaders;
        private readonly ILogger log;

        public MediaItemBuilder(IPluginConfiguration pluginConfiguration,
            IEnumerable<ISourceDataLoader> sourceDataLoaders, ILogManager logManager)
        {
            this.pluginConfiguration = pluginConfiguration;
            this.sourceDataLoaders = sourceDataLoaders;
            this.log = logManager.GetLogger(nameof(MediaItemBuilder));
        }

        public Task<Either<ProcessFailedResult, IMediaItem>> Identify(EmbyItemData embyItemData,
            IMediaItemType itemType)
        {
            return this.IdentifyAsync(embyItemData, itemType).MapAsync(sd => (IMediaItem)new MediaItem(embyItemData, itemType, sd));
        }

        public Task<Either<ProcessFailedResult, IMediaItem>> BuildMediaItem(IMediaItem rootMediaItem)
        {
            return AddDataFromSourcesAsync(Right<ProcessFailedResult, IMediaItem>(rootMediaItem).AsTask(),
                this.sourceDataLoaders.ToImmutableList());

            Task<Either<ProcessFailedResult, IMediaItem>> AddDataFromSourcesAsync(
                Task<Either<ProcessFailedResult, IMediaItem>> mediaItem,
                ImmutableList<ISourceDataLoader> sourceDataLoaders)
            {
                var sourceLoaderCount = sourceDataLoaders.Count;

                var mediaItemTask = sourceDataLoaders.Aggregate(mediaItem,
                    (miTask, l) =>
                        miTask.MapAsync(mi => mi.GetAllSourceData().Find(l.CanLoadFrom)
                            .MatchAsync(sd =>
                                {
                                    this.log.Debug($"Loading source data using {l.GetType().FullName}");
                                    return l.LoadFrom(mi, sd)
                                        .Map(e => e.Match(
                                            newSourceData =>
                                            {
                                                this.log.Debug($"Loaded {sd.Source.Name} source data: {sd.Identifier}");
                                                sourceDataLoaders = sourceDataLoaders.Remove(l);
                                                return mi.AddData(newSourceData).IfLeft(() =>
                                                {
                                                    this.log.Warn($"Failed to add source data: {sd.Identifier}");
                                                    return mi;
                                                });
                                            },
                                            fail =>
                                            {
                                                this.log.Debug($"Failed to load source data: {fail.Reason}");
                                                return mi;
                                            }));
                                },
                                () => mi)));

                return mediaItemTask.BindAsync(mi =>
                {
                    var wasSourceDataAdded = sourceLoaderCount != sourceDataLoaders.Count;

                    var mediaItemAsEither = Right<ProcessFailedResult, IMediaItem>(mi).AsTask();

                    return wasSourceDataAdded
                        ? AddDataFromSourcesAsync(mediaItemAsEither, sourceDataLoaders)
                        : mediaItemAsEither;
                });
            }
        }

        private Task<Either<ProcessFailedResult, ISourceData>> IdentifyAsync(EmbyItemData embyItemData, IMediaItemType itemType)
        {
            var identifyingSource = this.pluginConfiguration.FileStructureSource(itemType);

            return identifyingSource.GetEmbySourceDataLoader(embyItemData.ItemType)
                .BindAsync(l => l.LoadFrom(embyItemData));
        }
    }
}