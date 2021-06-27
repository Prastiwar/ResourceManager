﻿using System;

namespace ResourceManager.DataSource
{
    public interface ITrackedResource
    {
        public Type ResourceType { get; }

        public object Resource { get; }

        public object OriginalResource { get; }

        public ResourceState State { get; }
    }
}