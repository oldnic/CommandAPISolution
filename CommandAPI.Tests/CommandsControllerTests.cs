using System;
using Xunit;
using Moq;
using AutoMapper;
using CommandAPI.Models;
using CommandAPI.Data;
using System.Collections.Generic;
using System.Text;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Profiles;

namespace CommandAPI.Tests {
    public class CommandsControllerTests {

        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty() {
            //Arrange
            //We need to create an instance of our CommandsController class
            //CommandsController controller = new CommandsController( /* repository, AutoMapper */);
        }

        [Fact]
        public void GetCommandItems_Returns200OK_WhenDBIsEmpty() {

            //Arrange
            Mock<ICommandAPIRepo> mockRepo = new Mock<ICommandAPIRepo>();

            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));

            CommandsProfile realProfile = new CommandsProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            IMapper mapper = new Mapper(configuration);

            CommandsController controller = new CommandsController(mockRepo.Object, mapper);
        }

        private List<Command> GetCommands(int num) {
            List<Command> commands = new List<Command>();

            if (num > 0) {
                commands.Add(new Command {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
    }
}
