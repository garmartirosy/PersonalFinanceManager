using System.Threading.Tasks;
using Moq;
using Personal_Finance_Manager.Observers;
using Xunit;

namespace PersonalFinanceManager.Tests.Observers
{
    public class TransactionEventBusTests
    {
        [Fact]
        public async Task PublishAsync_NotifiesAllObserversOnce()
        {
            var eventBus = new TransactionEventBus();

            var observerA = new Mock<ITransactionObserver>();
            var observerB = new Mock<ITransactionObserver>();

            observerA.Setup(o => o.Handle(It.IsAny<TransactionEvent>()))
                     .Returns(Task.CompletedTask);

            observerB.Setup(o => o.Handle(It.IsAny<TransactionEvent>()))
                     .Returns(Task.CompletedTask);

            eventBus.Subscribe(observerA.Object);
            eventBus.Subscribe(observerB.Object);

            TransactionEvent transactionEvent = null;

            await eventBus.PublishAsync(transactionEvent);

            observerA.Verify(o => o.Handle(transactionEvent), Times.Once);
            observerB.Verify(o => o.Handle(transactionEvent), Times.Once);
        }

        [Fact]
        public async Task PublishAsync_CallsObserversInOrder()
        {
            var eventBus = new TransactionEventBus();

            int order = 0;
            int firstObserverCall = 0;
            int secondObserverCall = 0;

            var observer1 = new Mock<ITransactionObserver>();
            var observer2 = new Mock<ITransactionObserver>();

            observer1.Setup(o => o.Handle(It.IsAny<TransactionEvent>()))
                     .Returns(() =>
                     {
                         order++;
                         firstObserverCall = order;
                         return Task.CompletedTask;
                     });

            observer2.Setup(o => o.Handle(It.IsAny<TransactionEvent>()))
                     .Returns(() =>
                     {
                         order++;
                         secondObserverCall = order;
                         return Task.CompletedTask;
                     });

            eventBus.Subscribe(observer1.Object);
            eventBus.Subscribe(observer2.Object);

            TransactionEvent transactionEvent = null;

            await eventBus.PublishAsync(transactionEvent);

            Assert.Equal(1, firstObserverCall);
            Assert.Equal(2, secondObserverCall);
        }
    }
}
