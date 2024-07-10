
using Business.Handlers.TeamRoles.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.TeamRoles.Queries.GetTeamRoleQuery;
using Entities.Concrete;
using static Business.Handlers.TeamRoles.Queries.GetTeamRolesQuery;
using static Business.Handlers.TeamRoles.Commands.CreateTeamRoleCommand;
using Business.Handlers.TeamRoles.Commands;
using Business.Constants;
using static Business.Handlers.TeamRoles.Commands.UpdateTeamRoleCommand;
using static Business.Handlers.TeamRoles.Commands.DeleteTeamRoleCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class TeamRoleHandlerTests
    {
        Mock<ITeamRoleRepository> _teamRoleRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _teamRoleRepository = new Mock<ITeamRoleRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task TeamRole_GetQuery_Success()
        {
            //Arrange
            var query = new GetTeamRoleQuery();

            _teamRoleRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TeamRole, bool>>>())).ReturnsAsync(new TeamRole()
//propertyler buraya yazılacak
//{																		
//TeamRoleId = 1,
//TeamRoleName = "Test"
//}
);

            var handler = new GetTeamRoleQueryHandler(_teamRoleRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.TeamRoleId.Should().Be(1);

        }

        [Test]
        public async Task TeamRole_GetQueries_Success()
        {
            //Arrange
            var query = new GetTeamRolesQuery();

            _teamRoleRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<TeamRole, bool>>>()))
                        .ReturnsAsync(new List<TeamRole> { new TeamRole() { /*TODO:propertyler buraya yazılacak TeamRoleId = 1, TeamRoleName = "test"*/ } });

            var handler = new GetTeamRolesQueryHandler(_teamRoleRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<TeamRole>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task TeamRole_CreateCommand_Success()
        {
            TeamRole rt = null;
            //Arrange
            var command = new CreateTeamRoleCommand();
            //propertyler buraya yazılacak
            //command.TeamRoleName = "deneme";

            _teamRoleRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TeamRole, bool>>>()))
                        .ReturnsAsync(rt);

            _teamRoleRepository.Setup(x => x.Add(It.IsAny<TeamRole>())).Returns(new TeamRole());

            var handler = new CreateTeamRoleCommandHandler(_teamRoleRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _teamRoleRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task TeamRole_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateTeamRoleCommand();
            //propertyler buraya yazılacak 
            //command.TeamRoleName = "test";

            _teamRoleRepository.Setup(x => x.Query())
                                           .Returns(new List<TeamRole> { new TeamRole() { /*TODO:propertyler buraya yazılacak TeamRoleId = 1, TeamRoleName = "test"*/ } }.AsQueryable());

            _teamRoleRepository.Setup(x => x.Add(It.IsAny<TeamRole>())).Returns(new TeamRole());

            var handler = new CreateTeamRoleCommandHandler(_teamRoleRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task TeamRole_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateTeamRoleCommand();
            //command.TeamRoleName = "test";

            _teamRoleRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TeamRole, bool>>>()))
                        .ReturnsAsync(new TeamRole() { /*TODO:propertyler buraya yazılacak TeamRoleId = 1, TeamRoleName = "deneme"*/ });

            _teamRoleRepository.Setup(x => x.Update(It.IsAny<TeamRole>())).Returns(new TeamRole());

            var handler = new UpdateTeamRoleCommandHandler(_teamRoleRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _teamRoleRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task TeamRole_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteTeamRoleCommand();

            _teamRoleRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TeamRole, bool>>>()))
                        .ReturnsAsync(new TeamRole() { /*TODO:propertyler buraya yazılacak TeamRoleId = 1, TeamRoleName = "deneme"*/});

            _teamRoleRepository.Setup(x => x.Delete(It.IsAny<TeamRole>()));

            var handler = new DeleteTeamRoleCommandHandler(_teamRoleRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _teamRoleRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

