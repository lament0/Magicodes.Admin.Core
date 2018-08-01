﻿using System.Threading.Tasks;

namespace Magicodes.Admin.Core.Custom.LogInfos
{
    /// <summary>
    /// 交易日志仓储
    /// </summary>
    public interface ITransactionLogStore
    {
        Task SaveAsync(TransactionLog transactionLog);
    }
}