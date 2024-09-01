using MidProject.Models;
using MidProject.Models.Dto.Request;

namespace MidProject.Repositories.Interfaces
{
    public interface IPaymentTransaction
    {
        Task<IEnumerable<PaymentTransaction>> GetAllPaymentTransactions();
        Task<PaymentTransaction> GetPaymentTransactionById(int id);
        Task AddPaymentTransaction(PaymentTransactionDto paymentTransactionDto);
        Task UpdatePaymentTransaction(PaymentTransactionDto paymentTransactionDto);
        Task DeletePaymentTransaction(int id);
    }
}