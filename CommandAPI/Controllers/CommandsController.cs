using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;

namespace CommandAPI.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase {

        private readonly ICommandAPIRepo _repository;

        public CommandsController(ICommandAPIRepo repository) {
            _repository = repository;
        }

        /*[HttpGet]
        public ActionResult<IEnumerable<String>> Get() {
            return new string[] { "this", "is", "hard", "coded" };
        }*/

        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands() {
            IEnumerable<Command> commandItems = _repository.GetAllCommands();
            return Ok(commandItems);
        }

        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandById(int id) {
            Command commandItem = _repository.GetCommandById(id);

            if (commandItem == null) {
                return NotFound();
            }
            return Ok(commandItem);
        }
    }
}
