using System.Threading.Tasks;
using Moq;
using Personal_Finance_Manager.Undo;
using Personal_Finance_Manager.Undo.Operations;
using Xunit;

namespace PersonalFinanceManager.Tests.Undo
{
    public class UndoServiceTests
    {
        [Fact]
        public async Task ExecuteAndRemember_SavesUndo()
        {
            var service = new UndoService();
            var memento = new Mock<IOperationMemento>();

            var operation = new Mock<IUndoableOperation>();
            operation.Setup(o => o.Execute()).ReturnsAsync(memento.Object);

            await service.ExecuteAndRemember("user1", operation.Object);

            Assert.True(service.HasUndo("user1"));
        }

        [Fact]
        public async Task UndoLast_CallsUndoAndClears()
        {
            var service = new UndoService();
            var memento = new Mock<IOperationMemento>();

            var operation = new Mock<IUndoableOperation>();
            operation.Setup(o => o.Execute()).ReturnsAsync(memento.Object);

            await service.ExecuteAndRemember("user1", operation.Object);

            bool result = await service.UndoLast("user1");

            Assert.True(result);
            operation.Verify(o => o.Undo(memento.Object), Times.Once);
            Assert.False(service.HasUndo("user1"));
        }

        [Fact]
        public async Task UndoLast_WhenNothingSaved_ReturnsFalse()
        {
            var service = new UndoService();

            bool result = await service.UndoLast("user1");

            Assert.False(result);
        }
    }
}
