using MidProject.Data;
using MidProject.Models;
using MidProject.Repositories.Interfaces;

namespace MidProject.Repositories.Services
{
    public class PaymentTransactionServices : Repository<PaymentTransaction>, IPaymentTransaction
    {
        private readonly MidprojectDbContext _context;

        public PaymentTransactionServices(MidprojectDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
