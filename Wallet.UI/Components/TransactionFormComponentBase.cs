using System;
using System.Threading.Tasks;
using Wallet.Contracts.Requests;

namespace Wallet.UI.Components
{
    public abstract class TransactionFormComponentBase : CategoryAwareComponentBase<TransactionRequest>
    {
        protected void NavigateToTransactions() => NavigationManager.NavigateTo("transactions");

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Data.Date = DateTime.Now;
        }
    }
}