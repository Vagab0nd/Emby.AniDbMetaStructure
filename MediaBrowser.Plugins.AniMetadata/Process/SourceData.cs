﻿using System;
using LanguageExt;

namespace MediaBrowser.Plugins.AniMetadata.Process
{
    internal class SourceData<TData> : ISourceData<TData> where TData : class
    {
        private readonly TData data;

        public SourceData(ISource source, Option<int> id, IItemIdentifier identifier, TData data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            Id = id;
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }
        
        public ISource Source { get; }

        public Option<int> Id { get; }

        public IItemIdentifier Identifier { get; }

        public TData Data => this.data;

        object ISourceData.Data => Data;
        
        public Option<TRequestedData> GetData<TRequestedData>() where TRequestedData : class
        {
            return this.data as TRequestedData;
        }
    }
}