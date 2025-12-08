using AutoMapper;
using LearningManagementSystemApi.Dtos;
using LearningManagementSystemApi.Models;

namespace LearningManagementSystemApi.Profiles
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<AuthRegisterRequestDto, AppUser>();
            CreateMap<AppUser, AuthRegisterResponseDto>();

            CreateMap<CourseCreateRequestDto, Course>();
            CreateMap<Course, CourseCreateResponseDto>();

            CreateMap<Course, CourseResponseDto>()
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.AppUserId));
            CreateMap<Course, CourseWithLessonResponseDto>();

            CreateMap<Lesson, LessonResponseDto>();
            CreateMap<LessonCreateRequestDto, Lesson>();
            CreateMap<Lesson, LessonCreateResponseDto>();

        }
    }
}
