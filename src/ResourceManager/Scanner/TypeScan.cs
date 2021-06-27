using System;
using System.Collections.Generic;

namespace ResourceManager
{
    public class TypeScan
    {
        public TypeScan(Type scannedType, IEnumerable<Type> resultTypes)
        {
            ScannedType = scannedType;
            ResultTypes = resultTypes;
        }

        public Type ScannedType { get; }
        public IEnumerable<Type> ResultTypes { get; }
    }
}
