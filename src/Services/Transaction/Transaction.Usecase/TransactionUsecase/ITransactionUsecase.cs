using Transaction.Core.DataTransferObjects;

namespace Transaction.Usecase.TransactionUsecase;

public interface ITransactionUsecase
{
    public Task<TransactionDTO> DoTransaction(Transaction.Core.TransactionAggregate.Transaction transaction);
}