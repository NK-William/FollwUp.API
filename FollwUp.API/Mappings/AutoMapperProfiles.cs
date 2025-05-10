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
            CreateMap<Domain.Task, TrackerTaskDto>().ReverseMap();

            // Phase
            CreateMap<AddPhaseRequestDto, Domain.Phase>()
            .ForPath(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon!.Name))
            .ForPath(dest => dest.IconType, opt => opt.MapFrom(src => src.Icon!.Type))
            .ReverseMap();
            CreateMap<Domain.Phase, PhaseDto>()
            .ForPath(dest => dest.Icon!.Name, opt => opt.MapFrom(src => src.Icon))
            .ForPath(dest => dest.Icon!.Type, opt => opt.MapFrom(src => src.IconType))
            .ReverseMap();
            CreateMap<UpdatePhaseRequestDto, Domain.Phase>()
            .ForPath(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon!.Name))
            .ForPath(dest => dest.IconType, opt => opt.MapFrom(src => src.Icon!.Type))
            .ReverseMap();
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
            CreateMap<UpdateProfileRequestDto, Domain.Profile>().ReverseMap();
            CreateMap<UpdateProfileRequestDto, ProfileDto>().ReverseMap();
        }
    }
}
