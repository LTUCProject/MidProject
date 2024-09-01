using MidProject.Data;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class PaymentTransactionServices : IPaymentTransaction
    {
        private readonly MidprojectDbContext _context;

        public PaymentTransactionServices(MidprojectDbContext context) 
        {
            _context = context;
        }

        public Task AddPaymentTransaction(PaymentTransactionDto paymentTransactionDto)
        {
            throw new NotImplementedException();
        }

        public Task DeletePaymentTransaction(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PaymentTransaction>> GetAllPaymentTransactions()
        {
            throw new NotImplementedException();
        }

        public Task<PaymentTransaction> GetPaymentTransactionById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePaymentTransaction(PaymentTransactionDto paymentTransactionDto)
        {
            throw new NotImplementedException();
        }
    }
}
