using System.Collections.Generic;
using System.Linq;

namespace ResourceManager
{
    public class TypeScanResult
    {
        public TypeScanResult(IEnumerable<TypeScan> results) => Scans = results;

        public IEnumerable<TypeScan> Scans { get; }

        public bool HasResults => Scans.Any();
    }
}
