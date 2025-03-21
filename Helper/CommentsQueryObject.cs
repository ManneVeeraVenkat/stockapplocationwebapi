using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stockapplocation.Helper
{
    public class CommentsQueryObject
    {
        public string? Symbol { get; set; } = null;
        public bool IsDescending { get; set; } = true;

    }
}