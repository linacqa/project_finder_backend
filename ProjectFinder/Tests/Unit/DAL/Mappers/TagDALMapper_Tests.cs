using AwesomeAssertions;
using DAL.EF.Mappers;
using DalTag = global::DAL.DTO.Tag;
using DomainTag = global::Domain.Tag;

namespace Tests.Unit.DAL.Mappers;

public class TagDalMapperTests
{
    private readonly TagUOWMapper _sut = new();

    [Fact]
    public void Map_DomainToDal_AllFieldsCopied()
    {
        var domain = new DomainTag
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
        var dal = new DalTag
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
        _sut.Map((DomainTag?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalTag?)null).Should().BeNull();
    }
}