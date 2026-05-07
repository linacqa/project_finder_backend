using AwesomeAssertions;
using DAL.EF.Mappers;
using DalStep = global::DAL.DTO.Step;
using DomainStep = global::Domain.Step;

namespace Tests.Unit.DAL.Mappers;

public class StepDalMapperTests
{
    private readonly StepUOWMapper _sut = new();

    [Fact]
    public void Map_DomainToDal_AllFieldsCopied()
    {
        var domain = new DomainStep
        {
            Id = Guid.NewGuid(),
            Name = "name",
        };

        var dal = _sut.Map(domain)!;

        dal.Should().NotBeNull();
        dal.Id.Should().Be(domain.Id);
        dal.Name.Should().Be("name");
    }

    [Fact]
    public void Map_DalToDomain_AllFieldsCopied()
    {
        var dal = new DalStep
        {
            Id = Guid.NewGuid(),
            Name = "name",
        };

        var domain = _sut.Map(dal)!;

        domain.Should().NotBeNull();
        domain.Id.Should().Be(dal.Id);
        domain.Name.Should().Be("name");
    }

    [Fact]
    public void Map_NullDomain_ReturnsNull()
    {
        _sut.Map((DomainStep?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalStep?)null).Should().BeNull();
    }
}