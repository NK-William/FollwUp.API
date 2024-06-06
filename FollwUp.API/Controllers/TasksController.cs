using AutoMapper;
using FollwUp.API.Model.Domain;
using FollwUp.API.Model.DTO;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain = FollwUp.API.Model.Domain;

namespace FollwUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITaskRepository taskRepository;
        private readonly PhaseController phaseController;

        public TasksController(IMapper mapper, ITaskRepository taskRepository, PhaseController phaseController)
        {
            this.mapper = mapper;
            this.taskRepository = taskRepository;
            this.phaseController = phaseController;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTaskRequestDto addTaskRequestDto)
        {
            var taskDomainModel = mapper.Map<Domain.Task>(addTaskRequestDto);

            await taskRepository.CreateAsync(taskDomainModel);

            var taskDto = mapper.Map<TaskDto>(taskDomainModel);

            if (taskDto == null || taskDto.Id == null)
            {
                // Something went wrong
            }

            List<AddPhaseRequestDto> addPhaseRequestDto = addTaskRequestDto.Phases.Select(p => new AddPhaseRequestDto
            {
                Name = p.Name,
                Description = p.Description,
                Icon = p.Icon,
                Number = p.Number,
                Status = p.Status,
                TaskId = taskDto.Id
            }).ToList();

            var phaseDto = await phaseController.Create(addPhaseRequestDto);

            return Ok(taskDto);
        }
    }
}
