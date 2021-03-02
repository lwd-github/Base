﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Canal
{
    /// <summary>
    /// Canal消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CanalMessage<T>
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 操作（新增/修改/删除）
        /// </summary>
        public EActionType Action { get; set; }

        /// <summary>
        /// 修改前的实体
        /// </summary>
        public T Before { get; set; }

        /// <summary>
        /// 修改后的实体
        /// </summary>
        public T After { get; set; }

        /// <summary>
        /// 消息产生时间
        /// </summary>
        public DateTime Time { get; set; }
    }


    /// <summary>
    /// Canal消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CanalMessage : CanalMessage<object>
    {
    }
}
