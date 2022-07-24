using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Api.Domain;
using Wallet.Api.Models;

namespace Wallet.Api.Services
{
    public interface ITransactionService : IWalletService<Transaction>
    {
        Task<IEnumerable<Transaction>> GetAllAsync(string userId);

        Task<BalanceModel> GetBalanceAsync(string userId);

        Task<IEnumerable<Transaction>> SearchAsync(string userId, string text);
    }
}