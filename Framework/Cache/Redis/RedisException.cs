﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Cache.Redis
{
    public class RedisException : Exception
    {
        public RedisException()
        {

        }

        public RedisException(string message) : base(message)
        {

        }
    }
}
