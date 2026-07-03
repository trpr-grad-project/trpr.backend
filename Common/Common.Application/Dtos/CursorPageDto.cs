using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Application.Dtos
{
    public class CursorPageDto<TItem, TCursor>
    {
        public ICollection<TItem> Items { get; set; } = [];
        public TCursor? NextCursor { get; set; }
        public bool HasNextPage => NextCursor != null;
    }
}