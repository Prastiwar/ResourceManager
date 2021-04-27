using System.Collections.Generic;

namespace ResourceManager
{
    public class TypeScanResult
    {
        public TypeScanResult(IEnumerable<TypeScan> results) => ResultTypes = results;

        public IEnumerable<TypeScan> ResultTypes { get; }
    }
}
