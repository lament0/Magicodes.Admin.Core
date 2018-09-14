// ======================================================================
//   
//           Copyright (C) 2018-2020 ����������Ϣ�Ƽ����޹�˾    
//           All rights reserved
//   
//           filename : SmsVerificationCodeCacheExtensions.cs
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

using Abp.Runtime.Caching;

namespace Magicodes.App.Application.Users.Cache
{
    public static class SmsVerificationCodeCacheExtensions
    {
        public static ITypedCache<string, SmsVerificationCodeCacheItem> GetSmsVerificationCodeCache(
            this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, SmsVerificationCodeCacheItem>(SmsVerificationCodeCacheItem.CacheName);
        }
    }
}