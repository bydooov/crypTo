using CrypTo.Infrastructure.Contracts.Transactions;
using CrypTo.Infrastructure.Entities.Transactions;

namespace CrypTo.Infrastructure.Mapping
{
    public static class TransactionMapping
    {
        public static Transaction ToDomain(this CreateTransactionRequest request)
        {
            return new Transaction
            {
                Amount = request.Amount,
                Fee = request.Fee,
                ReceiverAddress = request.ReceiverAddress,
                SenderAddress = request.SenderAddress,
                Timestamp = DateTime.Now,
                Signature = request.Signature,
                Bytes = new Random().Next(250, 500)
            };
        }

        public static TransactionContract ToContract(this Transaction transaction)
        {
            return new TransactionContract
            {
                Amount = transaction.Amount,
                BlockIndex = transaction.BlockIndex,
                Fee = transaction.Fee,
                ReceiverAddress = transaction.ReceiverAddress,
                SenderAddress = transaction.SenderAddress,
                Timestamp = transaction.Timestamp,
                TransactionId = transaction.TransactionId,
            };
        }
    }
}
