using System.Text.Json;
using AutoMapper;
using EventBusKafka;
using Transaction.Core.DataTransferObjects;
using Transaction.Infrastructure.Data;

namespace Transaction.Usecase.TransactionUsecase;



public class TransactionUsecase : ITransactionUsecase
{
    private readonly RepositoryContext _context;
    private readonly IMapper _mapper;

    public TransactionUsecase(RepositoryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<TransactionDTO> DoTransaction(Core.TransactionAggregate.Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        // Created = 0
        // Pending = 1
        // Success = 2
        // Failed = 3


        UserBalanceForCreation userBalanceForCreation = new UserBalanceForCreation
        {
            UserId = transaction.UserId,
            TransactionId = transaction.Id,
            TransactionType = 1,
            Amount = transaction.Amount,
            CreatedAt = transaction.CreatedAt,
            UpdatedAt = transaction.UpdatedAt,
        };

        if (transaction.Status == 1)
        {
            userBalanceForCreation.TransactionType = 1;
        }
        else if (transaction.Status == 2)
        {
            userBalanceForCreation.TransactionType = 2;
        }
        else if (transaction.Status == 3)
        {
            userBalanceForCreation.TransactionType = 3;
        }

        string userBalanceForCreationByte = JsonSerializer.Serialize(userBalanceForCreation);
        await PushToKafkaAsync(userBalanceForCreationByte);

        TransactionDTO transactionDTO = new TransactionDTO
        {
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            PaymentMethod = transaction.PaymentMethod,
            SupplierId = transaction.SupplierId,
            UserId = transaction.UserId
        };


        return transactionDTO;
    }

    private async Task PushToKafkaAsync(string message)
    {
        await Producer.Produce(message);
    }
}