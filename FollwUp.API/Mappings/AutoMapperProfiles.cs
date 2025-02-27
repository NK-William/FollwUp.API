using AutoMapper;
using FollwUp.API.Model.DTO;
using Domain = FollwUp.API.Model.Domain;

namespace FollwUp.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Task
            CreateMap<AddTaskRequestDto, Domain.Task>().ReverseMap();
            CreateMap<UpdateTaskRequestDto, Domain.Task>().ReverseMap();
            CreateMap<Domain.Task, TaskDto>().ReverseMap();

            // Phase
            CreateMap<AddPhaseRequestDto, Domain.Phase>().ReverseMap();
            CreateMap<Domain.Phase, PhaseDto>().ReverseMap();
            CreateMap<UpdatePhaseRequestDto, Domain.Phase>().ReverseMap();
            CreateMap<UpdatePhaseRequestDto, PhaseDto>().ReverseMap();

            // Role
            CreateMap<AddRoleRequestDto, Domain.Role>().ReverseMap();
            CreateMap<Domain.Role, RoleDto>().ReverseMap();

            // Invitation
            CreateMap<AddInvitationRequestDto, Domain.Invitation>().ReverseMap();
            CreateMap<Domain.Invitation, InvitationDto>().ReverseMap();

            // Profile
            CreateMap<AddProfileRequestDto, Domain.Profile>().ReverseMap();
            CreateMap<Domain.Profile, ProfileDto>().ReverseMap();
        }
    }
}
