using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Better_Server {
    public static class lib {
        public static String GetTimestamp(DateTime value) {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
