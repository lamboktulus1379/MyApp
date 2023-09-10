using Transaction.Core.DataTransferObjects;

namespace Transaction.Usecase.TransactionUsecase;

public interface ITransactionUsecase
{
    public Task<Res> DoTransaction(Transaction.Core.TransactionAggregate.Transaction transaction);
    public Task<Res> GetTransactions();
}