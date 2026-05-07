using AwesomeAssertions;
using BLL.DTO;
using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;
using Moq;

using DalTag = global::DAL.DTO.Tag;

namespace Tests.Unit.BLL.Services;

public class TagService_Tests
{
    private readonly Mock<IAppUOW> _uow = new();
    private readonly Mock<ITagRepository> _repository = new();
    private readonly TagBLLMapper _mapper = new();
    private readonly TagService _sut;

    public TagService_Tests()
    {
        _uow.SetupGet(u => u.TagRepository).Returns(_repository.Object);
        _sut = new TagService(_uow.Object, _mapper);
    }

    [Fact]
    public async Task AllAsync_MapsResults()
    {
        var dalItems = new List<DalTag>
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
            .ReturnsAsync(new DalTag
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
        _repository.Setup(r => r.FindAsync(id)).ReturnsAsync((DalTag?)null);

        var result = await _sut.FindAsync(id);

        result.Should().BeNull();
    }

    [Fact]
    public void Add_DelegatesToRepository()
    {
        var bll = new Tag { Id = Guid.NewGuid(), Name = "d" };

        _sut.Add(bll);

        _repository.Verify(
            r => r.Add(It.Is<DalTag>(x => x.Id == bll.Id && x.Name == "d")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DelegatesToRepository_AndReturnsMapped()
    {
        var bll = new Tag { Id = Guid.NewGuid(), Name = "d"};
        _repository.Setup(r => r.UpdateAsync(It.IsAny<DalTag>()))
            .ReturnsAsync((DalTag e, Guid _) => e);

        var result = await _sut.UpdateAsync(bll);

        result.Id.Should().Be(bll.Id);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<DalTag>()), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_DelegatesToRepository()
    {
        var id = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalTag { Id = id, Name = "d" });

        await _sut.RemoveAsync(id);

        _repository.Verify(r => r.FindAsync(id), Times.Once);
        _repository.Verify(r => r.RemoveAsync(id), Times.Once);
    }

    [Fact]
    public void Remove_DelegatesToRepository()
    {
        var bll = new Tag { Id = Guid.NewGuid(), Name = "d" };

        _repository.Setup(r => r.Find(bll.Id, default))
            .Returns(new DalTag { Id = bll.Id, Name = "d" });

        _sut.Remove(bll);

        _repository.Verify(
            r => r.Find(bll.Id, default),
            Times.Once);
        _repository.Verify(
            r => r.Remove(It.Is<DalTag>(x => x.Id == bll.Id), default),
            Times.Once);
    }
}
