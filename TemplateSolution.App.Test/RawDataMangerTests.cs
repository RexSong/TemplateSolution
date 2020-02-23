using System;
using Xunit;
using AutoFixture;
using TemplateSolution.Repository.Domain;
using Moq;
using TemplateSolution.Repository.Interface;
using TemplateSolution.Repository;
using AutoFixture.AutoMoq;
using System.Linq.Expressions;

namespace TemplateSolution.App.Test
{
    public class RawDataMangerTests
    {
      
        [Fact]
        public void Get_Test()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var entity = fixture.Build<RawData>()
                .With(x => x.id, 1)
                .With(x => x.name, "abc")
                .Create();
            var repoMock = fixture.Freeze<Mock<IRepository<RawData>>>();
            repoMock.Setup(x => x.FindSingle(It.IsAny<Expression<Func<RawData, bool>>>())).Returns(entity);
            var unitMock = fixture.Freeze<Mock<UnitWork>>();
            var host = new RawDataManager(unitMock.Object, repoMock.Object);
            var a = host.Get(1);
            Assert.Equal(a.id, 1);
        }
    }
}
