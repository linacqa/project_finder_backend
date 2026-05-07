using AwesomeAssertions;
using BLL.DTO;
using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;
using Moq;

using DalStep = global::DAL.DTO.Step;

namespace Tests.Unit.BLL.Services;

public class StepService_Tests
{
    private readonly Mock<IAppUOW> _uow = new();
    private readonly Mock<IStepRepository> _repository = new();
    private readonly StepBLLMapper _mapper = new();
    private readonly StepService _sut;

    public StepService_Tests()
    {
        _uow.SetupGet(u => u.StepRepository).Returns(_repository.Object);
        _sut = new StepService(_uow.Object, _mapper);
    }

    [Fact]
    public async Task AllAsync_MapsResults()
    {
        var dalItems = new List<DalStep>
        {
            new() { Id = Guid.NewGuid(), Name = "a"},
            new() { Id = Guid.NewGuid(), Name = "b"},
        };
        _repository.Setup(r => r.AllAsync()).ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllAsync()).ToList();

        bllItems.Should().HaveCount(2);
        bllItems[0].Name.Should().Be("a");
        bllItems[1].Name.Should().Be("b");
        _repository.Verify(r => r.AllAsync(), Times.Once);
    }

    [Fact]
    public async Task FindAsync_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();
        _repository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalStep
            {
                Id = id, Name = "a"
            });

        var result = await _sut.FindAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        _repository.Verify(r => r.FindAsync(id), Times.Once);
    }

    [Fact]
    public async Task FindAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        var id = Guid.NewGuid();
        _repository.Setup(r => r.FindAsync(id)).ReturnsAsync((DalStep?)null);

        var result = await _sut.FindAsync(id);

        result.Should().BeNull();
    }

    [Fact]
    public void Add_DelegatesToRepository()
    {
        var bll = new Step { Id = Guid.NewGuid(), Name = "d" };

        _sut.Add(bll);

        _repository.Verify(
            r => r.Add(It.Is<DalStep>(x => x.Id == bll.Id && x.Name == "d")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DelegatesToRepository_AndReturnsMapped()
    {
        var bll = new Step { Id = Guid.NewGuid(), Name = "d"};
        _repository.Setup(r => r.UpdateAsync(It.IsAny<DalStep>()))
            .ReturnsAsync((DalStep e, Guid _) => e);

        var result = await _sut.UpdateAsync(bll);

        result.Id.Should().Be(bll.Id);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<DalStep>()), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_DelegatesToRepository()
    {
        var id = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalStep { Id = id, Name = "d" });

        await _sut.RemoveAsync(id);

        _repository.Verify(r => r.FindAsync(id), Times.Once);
        _repository.Verify(r => r.RemoveAsync(id), Times.Once);
    }

    [Fact]
    public void Remove_DelegatesToRepository()
    {
        var bll = new Step { Id = Guid.NewGuid(), Name = "d" };

        _repository.Setup(r => r.Find(bll.Id, default))
            .Returns(new DalStep { Id = bll.Id, Name = "d" });

        _sut.Remove(bll);

        _repository.Verify(
            r => r.Find(bll.Id, default),
            Times.Once);
        _repository.Verify(
            r => r.Remove(It.Is<DalStep>(x => x.Id == bll.Id), default),
            Times.Once);
    }
}
