﻿using System;
using System.Collections.Generic;
using Hawk.Core.Utils;
using Hawk.Core.Utils.Plugins;

namespace Hawk.Core.Connectors
{
    [Interface( "文件导入导出基本接口" )]
    public interface IFileConnector
    {
        #region Properties


 
        /// <summary>
        /// 扩展文件名
        /// </summary>
        string ExtentFileName { get; }
        /// <summary>
        /// 要读写的文件名
        /// </summary>
        string FileName { get; set; }


        #endregion

      

        #region Public Methods
      

         /// <summary>
         /// 迭代式文件操作
         /// </summary>
         /// <param name="path"></param>
         IEnumerable<IFreeDocument> WriteData(IEnumerable<IFreeDocument> datas );
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
         IEnumerable<IFreeDocument> ReadFile(Action<int> alreadyGetSize=null);

        /// <summary>
        /// 获取得到的文本
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        string GetString(IEnumerable<IFreeDocument> datas);

        #endregion
    }
}