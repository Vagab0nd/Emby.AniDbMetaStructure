﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using MediaBrowser.Plugins.AniMetadata.Process.Providers;

namespace MediaBrowser.Plugins.AniMetadata.EntryPoints
{
    public class SeriesProviderEntryPoint : IRemoteMetadataProvider<Series, SeriesInfo>, IHasOrder
    {
        private readonly SeriesProvider seriesProvider;

        public SeriesProviderEntryPoint(IApplicationHost applicationHost)
        {
            this.seriesProvider =
                DependencyConfiguration.Resolve<SeriesProvider>(applicationHost);
        }

        public int Order => this.seriesProvider.Order;

        public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeriesInfo searchInfo,
            CancellationToken cancellationToken)
        {
            return this.seriesProvider.GetSearchResults(searchInfo, cancellationToken);
        }

        public Task<MetadataResult<Series>> GetMetadata(SeriesInfo info, CancellationToken cancellationToken)
        {
            return this.seriesProvider.GetMetadata(info, cancellationToken);
        }

        public string Name => this.seriesProvider.Name;

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            return this.seriesProvider.GetImageResponse(url, cancellationToken);
        }
    }
}