using ResourceManager.Data;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ResourceManager
{
    public class IdentifiableComparer : Comparer<IIdentifiable>
    {
        public override int Compare([AllowNull] IIdentifiable x, [AllowNull] IIdentifiable y)
        {
            int xId = (int)x.Id;
            int yId = (int)y.Id;
            if (xId == yId)
            {
                return 0;
            }
            else if (xId < yId)
            {
                return -1;
            }
            return 1;
        }
    }
}
