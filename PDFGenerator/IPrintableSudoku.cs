using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFGenerator
{
    public interface IPrintableSudoku
    {
        public int[] Data { get; }
        public int Size { get; }
    }
}
