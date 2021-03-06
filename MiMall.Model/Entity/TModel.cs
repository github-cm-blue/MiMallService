﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiMall.Model.Entity
{
    public class TModel<T>
    {
        /// <summary>
        /// 0表示正常
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}
