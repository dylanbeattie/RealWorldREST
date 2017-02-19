using System.Collections.Generic;

namespace RealWorldRest.Data {
    public class Page<T> {
        public int Index { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public IList<T> Items { get; set; }
    }
}