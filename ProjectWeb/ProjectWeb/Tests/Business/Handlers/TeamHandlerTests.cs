
using Business.Handlers.Teams.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Teams.Queries.GetTeamQuery;
using Entities.Concrete;
using static Business.Handlers.Teams.Queries.GetTeamsQuery;
using static Business.Handlers.Teams.Commands.CreateTeamCommand;
using Business.Handlers.Teams.Commands;
using Business.Constants;
using static Business.Handlers.Teams.Commands.UpdateTeamCommand;
using static Business.Handlers.Teams.Commands.DeleteTeamCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class TeamHandlerTests
    {
        Mock<ITeamRepository> _teamRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _teamRepository = new Mock<ITeamRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Team_GetQuery_Success()
        {
            //Arrange
            var query = new GetTeamQuery();

            _teamRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Team, bool>>>())).ReturnsAsync(new Team()
//propertyler buraya yazılacak
//{																		
//TeamId = 1,
//TeamName = "Test"
//}
);

            var handler = new GetTeamQueryHandler(_teamRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.TeamId.Should().Be(1);

        }

        [Test]
        public async Task Team_GetQueries_Success()
        {
            //Arrange
            var query = new GetTeamsQuery();

            _teamRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Team, bool>>>()))
                        .ReturnsAsync(new List<Team> { new Team() { /*TODO:propertyler buraya yazılacak TeamId = 1, TeamName = "test"*/ } });

            var handler = new GetTeamsQueryHandler(_teamRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Team>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Team_CreateCommand_Success()
        {
            Team rt = null;
            //Arrange
            var command = new CreateTeamCommand();
            //propertyler buraya yazılacak
            //command.TeamName = "deneme";

            _teamRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Team, bool>>>()))
                        .ReturnsAsync(rt);

            _teamRepository.Setup(x => x.Add(It.IsAny<Team>())).Returns(new Team());

            var handler = new CreateTeamCommandHandler(_teamRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _teamRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Team_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateTeamCommand();
            //propertyler buraya yazılacak 
            //command.TeamName = "test";

            _teamRepository.Setup(x => x.Query())
                                           .Returns(new List<Team> { new Team() { /*TODO:propertyler buraya yazılacak TeamId = 1, TeamName = "test"*/ } }.AsQueryable());

            _teamRepository.Setup(x => x.Add(It.IsAny<Team>())).Returns(new Team());

            var handler = new CreateTeamCommandHandler(_teamRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Team_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateTeamCommand();
            //command.TeamName = "test";

            _teamRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Team, bool>>>()))
                        .ReturnsAsync(new Team() { /*TODO:propertyler buraya yazılacak TeamId = 1, TeamName = "deneme"*/ });

            _teamRepository.Setup(x => x.Update(It.IsAny<Team>())).Returns(new Team());

            var handler = new UpdateTeamCommandHandler(_teamRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _teamRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Team_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteTeamCommand();

            _teamRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Team, bool>>>()))
                        .ReturnsAsync(new Team() { /*TODO:propertyler buraya yazılacak TeamId = 1, TeamName = "deneme"*/});

            _teamRepository.Setup(x => x.Delete(It.IsAny<Team>()));

            var handler = new DeleteTeamCommandHandler(_teamRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _teamRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

