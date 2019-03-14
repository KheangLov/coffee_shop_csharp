using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    abstract class Db_Insert : MyInter
    {
        public abstract void insert();
        public abstract void update(int id);
    }
}
