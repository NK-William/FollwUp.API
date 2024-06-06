using AutoMapper;
using FollwUp.API.Model.DTO;

namespace FollwUp.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Task
            CreateMap<AddTaskRequestDto, Model.Domain.Task>().ReverseMap();
            CreateMap<Model.Domain.Task, TaskDto>().ReverseMap();

            // Phase
            CreateMap<AddPhaseRequestDto, Model.Domain.Phase>().ReverseMap();
            CreateMap<Model.Domain.Phase, PhaseDto>().ReverseMap();
        }
    }
}
