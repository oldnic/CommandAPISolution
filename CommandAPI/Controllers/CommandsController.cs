using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using CommandAPI.Models;
using AutoMapper;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase {

        private readonly ICommandAPIRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandAPIRepo repository, IMapper mapper) {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto) {
            Command commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            CommandReadDto commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto) {
            Command commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null) {
                return NotFound();
            }
            _mapper.Map(commandUpdateDto, commandModelFromRepo);
            _repository.UpdateCommand(commandModelFromRepo);
            _repository.SaveChanges();
            return NoContent();
        }
        /*[HttpGet]
        public ActionResult<IEnumerable<String>> Get() {
            return new string[] { "this", "is", "hard", "coded" };
        }*/

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc) {
            Command commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null) {
                return NotFound();
            }
            CommandUpdateDto commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);
            if (!TryValidateModel(commandToPatch)) {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(commandToPatch, commandModelFromRepo);
            _repository.UpdateCommand(commandModelFromRepo);
            _repository.SaveChanges();
            return NoContent();
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands() {
            IEnumerable<Command> commandItems = _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id) {
            Command commandItem = _repository.GetCommandById(id);

            if (commandItem == null) {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id) {
            Command commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null) {
                return NotFound();
            }
            _repository.DeleteCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}
