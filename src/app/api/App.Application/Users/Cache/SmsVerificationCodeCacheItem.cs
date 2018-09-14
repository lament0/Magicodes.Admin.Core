// ======================================================================
//   
//           Copyright (C) 2018-2020 ����������Ϣ�Ƽ����޹�˾    
//           All rights reserved
//   
//           filename : SmsVerificationCodeCacheItem.cs
//           description :
//   
//           created by ѩ�� at  2018-09-14 10:09
//           Mail: wenqiang.li@xin-lai.com
//           QQȺ��85318032������������
//           Blog��http://www.cnblogs.com/codelove/
//           GitHub��https://github.com/xin-lai
//           Home��http://xin-lai.com
//   
// ======================================================================

using System;
using Abp.Timing;

namespace Magicodes.App.Application.Users.Cache
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class SmsVerificationCodeCacheItem
    {
        public const string CacheName = "AppSmsVerificationCodeCache";

        /// <summary>
        /// </summary>
        public SmsVerificationCodeCacheItem()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="code"></param>
        public SmsVerificationCodeCacheItem(string code)
        {
            Code = code;
            CreationTime = Clock.Now;
        }

        /// <summary>
        ///     ��֤��
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     ����ʱ��
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}