using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calendar4
{
    internal sealed record SearchResultItem(
        DateTime Date,
        string Title,
        string Detail)
    {
        public override string ToString()
        {
            return $"{Title}\n{Detail}";
        }
    }
}
