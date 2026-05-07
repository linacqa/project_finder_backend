using AwesomeAssertions;
using BLL.DTO;
using BLL.Mappers;
using Tag = global::BLL.DTO.Tag;
using DalTag = global::DAL.DTO.Tag;

namespace Tests.Unit.BLL.Mappers;

public class TagBLLMapper_Tests
{
    private readonly TagBLLMapper _sut = new();

    [Fact]
    public void Map_DalToBll_AllFieldsCopied()
    {
        var dal = new DalTag
        {
            Id = Guid.NewGuid(),
            Name = "name",
        };

        var bll = _sut.Map(dal)!;

        bll.Id.Should().Be(dal.Id);
        bll.Name.Should().Be("name");
    }

    [Fact]
    public void Map_BllToDal_AllFieldsCopied()
    {
        var bll = new Tag
        {
            Id = Guid.NewGuid(),
            Name = "name",
        };

        var dal = _sut.Map(bll)!;

        dal.Id.Should().Be(bll.Id);
        dal.Name.Should().Be("name");
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalTag?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullBll_ReturnsNull()
    {
        _sut.Map((Tag?)null).Should().BeNull();
    }
}