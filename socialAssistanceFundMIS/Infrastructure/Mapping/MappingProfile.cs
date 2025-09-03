using AutoMapper;
using socialAssistanceFundMIS.DTOs;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.ViewModels;

namespace socialAssistanceFundMIS.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Applicant mappings
            CreateMap<Applicant, ApplicantDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.SexName, opt => opt.MapFrom(src => src.Sex!.Name))
                .ForMember(dest => dest.MaritalStatusName, opt => opt.MapFrom(src => src.MaritalStatus!.Name))
                .ForMember(dest => dest.VillageName, opt => opt.MapFrom(src => src.Village!.Name))
                .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers));

            CreateMap<ApplicantDTO, Applicant>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.MaritalStatusId, opt => opt.MapFrom(src => src.MaritalStatusId))
                .ForMember(dest => dest.VillageId, opt => opt.MapFrom(src => src.VillageId));

            CreateMap<Applicant, ApplicantViewModel>()
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.MaritalStatusId, opt => opt.MapFrom(src => src.MaritalStatusId))
                .ForMember(dest => dest.VillageId, opt => opt.MapFrom(src => src.VillageId));

            CreateMap<ApplicantViewModel, Applicant>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Dob))
                .ForMember(dest => dest.MaritalStatusId, opt => opt.MapFrom(src => src.MaritalStatusId))
                .ForMember(dest => dest.VillageId, opt => opt.MapFrom(src => src.VillageId));

            // Phone number mappings
            CreateMap<ApplicantPhoneNumber, ApplicantPhoneNumberDTO>()
                .ForMember(dest => dest.PhoneNumberTypeName, opt => opt.MapFrom(src => src.PhoneNumberType!.Name));

            CreateMap<ApplicantPhoneNumberDTO, ApplicantPhoneNumber>();

            // Application mappings
            CreateMap<Application, ApplicationDTO>()
                .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => src.Applicant!.FullName))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status!.Name))
                .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.AssistanceProgram!.Name));

            CreateMap<ApplicationDTO, Application>();

            CreateMap<Application, ApplicationViewModel>();
            CreateMap<ApplicationViewModel, Application>();

            // Lookup entity mappings
            CreateMap<Sex, SexDTO>();
            CreateMap<SexDTO, Sex>();

            CreateMap<MaritalStatus, MaritalStatusDTO>();
            CreateMap<MaritalStatusDTO, MaritalStatus>();

            CreateMap<PhoneNumberType, PhoneNumberTypeDTO>();
            CreateMap<PhoneNumberTypeDTO, PhoneNumberType>();

            CreateMap<Status, StatusDTO>();
            CreateMap<StatusDTO, Status>();

            CreateMap<AssistanceProgram, AssistanceProgramDTO>();
            CreateMap<AssistanceProgramDTO, AssistanceProgram>();

            CreateMap<GeographicLocation, GeographicLocationDTO>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type!.Name));

            CreateMap<GeographicLocationDTO, GeographicLocation>();

            CreateMap<GeographicLocationType, GeographicLocationTypeDTO>();
            CreateMap<GeographicLocationTypeDTO, GeographicLocationType>();

            CreateMap<Officer, OfficerDTO>();
            CreateMap<OfficerDTO, Officer>();

            CreateMap<Designation, DesignationDTO>();
            CreateMap<DesignationDTO, Designation>();

            CreateMap<OfficialRecord, OfficialRecordDTO>();
            CreateMap<OfficialRecordDTO, OfficialRecord>();

            // Reverse mappings for collections
            CreateMap<List<Applicant>, List<ApplicantDTO>>();
            CreateMap<List<ApplicantDTO>, List<Applicant>>();

            CreateMap<List<Application>, List<ApplicationDTO>>();
            CreateMap<List<ApplicationDTO>, List<Application>>();
        }
    }
}

