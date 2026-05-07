using AwesomeAssertions;
using BLL.DTO;
using BLL.Mappers;
using Step = global::BLL.DTO.Step;
using DalStep = global::DAL.DTO.Step;

namespace Tests.Unit.BLL.Mappers;

public class StepBLLMapper_Tests
{
    private readonly StepBLLMapper _sut = new();

    [Fact]
    public void Map_DalToBll_AllFieldsCopied()
    {
        var dal = new DalStep
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
        var bll = new Step
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
        _sut.Map((DalStep?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullBll_ReturnsNull()
    {
        _sut.Map((Step?)null).Should().BeNull();
    }
}