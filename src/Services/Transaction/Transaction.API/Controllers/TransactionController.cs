using Microsoft.AspNetCore.Mvc;
using Transaction.Core.DataTransferObjects;
using Transaction.Infrastructure.Data;
using Transaction.Usecase.TransactionUsecase;

namespace Transaction.API;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly RepositoryContext _context;
    private readonly ITransactionUsecase _transactionUsecase;
    public TransactionController(RepositoryContext context, ITransactionUsecase transactionUsecase)
    {
        _context = context;
        _transactionUsecase = transactionUsecase;
    }

    // POST: api/Transactions
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<TransactionDTO>> PostTransaction(TransactionDTO transactionDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var transaction = new Transaction.Core.TransactionAggregate.Transaction
        {
            Amount = transactionDTO.Amount,
            Currency = "IDR",
            PaymentMethod = transactionDTO.PaymentMethod,
            SupplierId = Guid.NewGuid().ToString(),
            UserId = transactionDTO.UserId,
            Status = 0,
        };

        var response = _transactionUsecase.DoTransaction(transaction);

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
}