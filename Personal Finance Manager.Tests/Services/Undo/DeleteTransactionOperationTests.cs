using System;
using System.Threading.Tasks;
using Moq;
using Personal_Finance_Manager.Data.Repositories;
using Personal_Finance_Manager.Undo;
using Personal_Finance_Manager.Undo.Mementos;
using Personal_Finance_Manager.Undo.Operations;
using Xunit;

namespace PersonalFinanceManager.Tests.Undo.Operations
{
    public class DeleteTransactionOperationTests
    {
        [Fact]
        public async Task Execute_CallsDeleteTransaction_AndReturnsMemento()
        {
            var transactionRepo = new Mock<ITransactionRepository>();

            string userId = "user-1";
            int transactionId = 123;

            transactionRepo
                .Setup(r => r.DeleteTransaction(transactionId, userId))
                .ReturnsAsync(true);

            var operation = new DeleteTransactionOperation(transactionRepo.Object, userId, transactionId);

            var memento = await operation.Execute();

            transactionRepo.Verify(r => r.DeleteTransaction(transactionId, userId), Times.Once);

            var deleteMemento = Assert.IsType<DeleteTransactionMemento>(memento);
            Assert.Equal(transactionId, deleteMemento.TransactionId);
        }

        [Fact]
        public async Task Undo_CallsUndoDeleteTransaction()
        {
            var transactionRepo = new Mock<ITransactionRepository>();

            string userId = "user-1";
            int transactionId = 456;

            var operation = new DeleteTransactionOperation(transactionRepo.Object, userId, transactionId);

            var memento = new DeleteTransactionMemento(DateTime.UtcNow, transactionId);

            await operation.Undo(memento);

            transactionRepo.Verify(r => r.UndoDeleteTransaction(userId, transactionId), Times.Once);
        }

        [Fact]
        public async Task Undo_WrongMemento_ThrowsError()
        {
            var transactionRepo = new Mock<ITransactionRepository>();

            string userId = "user-1";
            int transactionId = 789;

            var operation = new DeleteTransactionOperation(transactionRepo.Object, userId, transactionId);

            var wrongMemento = new Mock<IOperationMemento>().Object;

            await Assert.ThrowsAsync<InvalidCastException>(() => operation.Undo(wrongMemento));
        }
    }
}
