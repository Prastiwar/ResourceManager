using ResourceManager.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ResourceManager
{
    public class IdentifiableComparer : Comparer<IIdentifiable>
    {
        public override int Compare([AllowNull] IIdentifiable x, [AllowNull] IIdentifiable y)
        {
            int xId = x.Id is int xInteger ? xInteger : Convert.ToInt32(x.Id);
            int yId = y.Id is int yInteger ? yInteger : Convert.ToInt32(y.Id);
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
