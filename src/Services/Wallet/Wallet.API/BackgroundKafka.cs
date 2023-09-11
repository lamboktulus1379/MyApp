using System.Text.Json;
using AutoMapper;
using Wallet.Infrastructure.LoggerService;
using Wallet.Core.DataTransferObjects;
using Wallet.Core.Models;
using Wallet.Infrastructure.Data;
using EventBusKafka;

namespace Wallet.API
{
    class BackgroundKafka : BackgroundService
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IServiceScopeFactory _serviceFactory;

        public BackgroundKafka(IMapper mapper, IServiceScopeFactory serviceScopeFactory, ILoggerManager logger)
        {

            Console.WriteLine("Message received.");
            _logger = logger;
            _mapper = mapper;
            _serviceFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInfo("Start Task >>>");
            Console.WriteLine("Message received.");
            //new Thread(() => StartConsumerLoop(cancellationToken)).Start();
            await StartConsumerLoop(cancellationToken);
            //Task.Run(() => StartConsumerLoop(cancellationToken));
            //return Task.CompletedTask;
        }

        private async Task StartConsumerLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // TODO: Change consumer to Dependency Injection
                string message = Consumer.Consume();

                if (message != "")
                    _logger.LogInfo("Message >>> " + message);
                UserBalanceForCreation userBalanceForCreation = JsonSerializer.Deserialize<UserBalanceForCreation>(message);

                _logger.LogInfo("User Id >>> " + userBalanceForCreation.UserId);



                using (var scope = _serviceFactory.CreateScope())
                {
                    // TODO: Use UserBalanceFactory Instead of creating new instance of dbcontext
                    var dbContext = scope.ServiceProvider.GetService<WalletContext>();

                    User user = await dbContext.Users.FindAsync(userBalanceForCreation.UserId);
                    Console.WriteLine("Users: " + user);
                    // 
                    double amount = userBalanceForCreation.Amount;
                    userBalanceForCreation.PreviousBalance = user.Balance;
                    userBalanceForCreation.CurrentBalance = user.Balance - amount;
                    // 

                    user.Balance = userBalanceForCreation.CurrentBalance;
                    dbContext.Users.Update(user);

                    var userBalanceForCreationEntity = _mapper.Map<UserBalance>(userBalanceForCreation);

                    UserBalanceLog userBalanceLog = new UserBalanceLog
                    {
                        Amount = userBalanceForCreation.Amount,
                        CurrentBalance = user.Balance - amount,
                        TransactionType = userBalanceForCreation.TransactionType,
                    };

                    userBalanceForCreationEntity.UserBalanceLogs.Add(userBalanceLog);

                    dbContext.UserBalance.Add(userBalanceForCreationEntity);

                    await dbContext.SaveChangesAsync();

                }
                await Task.CompletedTask;
            }

        }
    }

}
