using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public partial class ProductOrder
    {
        public decimal Sum => Count * (int)Product.Price;
    }
}
