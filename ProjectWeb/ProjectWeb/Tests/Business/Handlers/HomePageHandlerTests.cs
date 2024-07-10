
using Business.Handlers.HomePages.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.HomePages.Queries.GetHomePageQuery;
using Entities.Concrete;
using static Business.Handlers.HomePages.Queries.GetHomePagesQuery;
using static Business.Handlers.HomePages.Commands.CreateHomePageCommand;
using Business.Handlers.HomePages.Commands;
using Business.Constants;
using static Business.Handlers.HomePages.Commands.UpdateHomePageCommand;
using static Business.Handlers.HomePages.Commands.DeleteHomePageCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class HomePageHandlerTests
    {
        Mock<IHomePageRepository> _homePageRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _homePageRepository = new Mock<IHomePageRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task HomePage_GetQuery_Success()
        {
            //Arrange
            var query = new GetHomePageQuery();

            _homePageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<HomePage, bool>>>())).ReturnsAsync(new HomePage()
//propertyler buraya yazılacak
//{																		
//HomePageId = 1,
//HomePageName = "Test"
//}
);

            var handler = new GetHomePageQueryHandler(_homePageRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.HomePageId.Should().Be(1);

        }

        [Test]
        public async Task HomePage_GetQueries_Success()
        {
            //Arrange
            var query = new GetHomePagesQuery();

            _homePageRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<HomePage, bool>>>()))
                        .ReturnsAsync(new List<HomePage> { new HomePage() { /*TODO:propertyler buraya yazılacak HomePageId = 1, HomePageName = "test"*/ } });

            var handler = new GetHomePagesQueryHandler(_homePageRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<HomePage>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task HomePage_CreateCommand_Success()
        {
            HomePage rt = null;
            //Arrange
            var command = new CreateHomePageCommand();
            //propertyler buraya yazılacak
            //command.HomePageName = "deneme";

            _homePageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<HomePage, bool>>>()))
                        .ReturnsAsync(rt);

            _homePageRepository.Setup(x => x.Add(It.IsAny<HomePage>())).Returns(new HomePage());

            var handler = new CreateHomePageCommandHandler(_homePageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _homePageRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task HomePage_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateHomePageCommand();
            //propertyler buraya yazılacak 
            //command.HomePageName = "test";

            _homePageRepository.Setup(x => x.Query())
                                           .Returns(new List<HomePage> { new HomePage() { /*TODO:propertyler buraya yazılacak HomePageId = 1, HomePageName = "test"*/ } }.AsQueryable());

            _homePageRepository.Setup(x => x.Add(It.IsAny<HomePage>())).Returns(new HomePage());

            var handler = new CreateHomePageCommandHandler(_homePageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task HomePage_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateHomePageCommand();
            //command.HomePageName = "test";

            _homePageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<HomePage, bool>>>()))
                        .ReturnsAsync(new HomePage() { /*TODO:propertyler buraya yazılacak HomePageId = 1, HomePageName = "deneme"*/ });

            _homePageRepository.Setup(x => x.Update(It.IsAny<HomePage>())).Returns(new HomePage());

            var handler = new UpdateHomePageCommandHandler(_homePageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _homePageRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task HomePage_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteHomePageCommand();

            _homePageRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<HomePage, bool>>>()))
                        .ReturnsAsync(new HomePage() { /*TODO:propertyler buraya yazılacak HomePageId = 1, HomePageName = "deneme"*/});

            _homePageRepository.Setup(x => x.Delete(It.IsAny<HomePage>()));

            var handler = new DeleteHomePageCommandHandler(_homePageRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _homePageRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

