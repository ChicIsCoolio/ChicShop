﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicShop.Chic
{
    public class ChicRatios
    {
        public static float Get(float original) => original / 512;
        public static float Get1024(float original) => original / 1024;
    }
}
