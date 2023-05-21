using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES.Logic
{
    public class KeyGenerator
    {
        public byte[] Generate3DesKeys()
        {
            byte[] key = new byte[8];

            Random random = new Random();
            random.NextBytes(key);

            return key;
        }
    }
}
