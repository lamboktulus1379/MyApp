using EventBusKafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Transaction.Core.DataTransferObjects;
using Transaction.Infrastructure.Data;

namespace Transaction.API;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly RepositoryContext _context;
    public TransactionController(RepositoryContext context)
    {
        _context = context;
    }

    // POST: api/Transactions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<TransactionDTO>> PostTransaction(TransactionDTO transactionDTO)
    {
        var transaction = new Transaction.Core.TransactionAggregate.Transaction
        {
            Amount = transactionDTO.Amount,
            Currency = "IDR",
            PaymentMethod = transactionDTO.PaymentMethod,
            SupplierId = Guid.NewGuid().ToString(),
            UserId = transactionDTO.UserId,
            Status = 0,
        };

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

        string userBalanceForCreationByte = JsonConvert.SerializeObject(userBalanceForCreation);
        await PushToKafkaAsync(userBalanceForCreationByte);


        return CreatedAtAction(
            nameof(GetTransaction),
            new { id = transaction.Id },
            TransactionToDTO(transaction));
    }

    // GET: api/Transactions/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDTO>> GetTransaction(long id)
    {
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction == null)
        {
            return NotFound();
        }

        return TransactionToDTO(transaction);
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

    private async Task PushToKafkaAsync(string message)
    {
        await Producer.Produce(message);
    }
}