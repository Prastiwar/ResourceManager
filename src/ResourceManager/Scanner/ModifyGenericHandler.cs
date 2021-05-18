using System;

namespace ResourceManager
{
    public delegate Type ModifyGenericHandler(Type scannedType, Type assemblyType, Type acceptedGenericType);
}
