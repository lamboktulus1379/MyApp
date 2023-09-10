using System.Text.Json;
using AutoMapper;
using Azure;
using EventBusKafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Transaction.Core.DataTransferObjects;
using Transaction.Infrastructure.Data;
using Transaction.Infrastructure.UserClient;

namespace Transaction.Usecase.TransactionUsecase;



public class TransactionUsecase : ITransactionUsecase
{
    private readonly RepositoryContext _context;
    private readonly IMapper _mapper;
    private readonly IUserClient _userClient;
    private readonly ILogger<TransactionUsecase> _logger;


    public TransactionUsecase(RepositoryContext context, IMapper mapper, IUserClient userClient, ILogger<TransactionUsecase> logger)
    {
        _context = context;
        _mapper = mapper;
        _userClient = userClient;
        _logger = logger;
    }
    public async Task<Res> DoTransaction(Core.TransactionAggregate.Transaction transaction)
    {
        var user = await _userClient.GetUser(transaction.UserId);
        var response = new Res
        {
            ResponseMessage = "OK",
            ResponseCode = "200",
        };
        string userStr = JsonSerializer.Serialize(user);
        _logger.LogInformation("User: " + userStr, userStr);
        if (user.Balance < transaction.Amount)
        {
            response.ResponseMessage = "Unsufficient balance";
            response.ResponseCode = "400";
            return response;
        };

        using var trx = _context.Database.BeginTransaction();
        try
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

            trx.Commit();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }





        TransactionDTO transactionDTO = new TransactionDTO
        {
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            PaymentMethod = transaction.PaymentMethod,
            SupplierId = transaction.SupplierId,
            UserId = transaction.UserId
        };


        return response;
    }

    public async Task<Res> GetTransactions()
    {
        var response = new Res { ResponseCode = "200", ResponseMessage = "Success" };
        var transactions = await _context.Transactions
        .Select(x => TransactionToDTO(x))
                .ToListAsync();

        response.Data = transactions;

        return response;
    }

    private async Task PushToKafkaAsync(string message)
    {
        await Producer.Produce(message);
    }

    private static TransactionDTO TransactionToDTO(Transaction.Core.TransactionAggregate.Transaction transaction) =>
       new TransactionDTO
       {
           Amount = transaction.Amount,
           Currency = transaction.Currency,
           PaymentMethod = transaction.PaymentMethod,
           SupplierId = transaction.SupplierId,
           UserId = transaction.UserId,
       };
}