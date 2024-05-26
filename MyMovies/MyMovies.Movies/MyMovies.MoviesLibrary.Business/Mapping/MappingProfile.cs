using AutoMapper;
using MyMovies.MoviesLibrary.Business.Model;
using MyMovies.MoviesLibrary.Domain;

namespace MyMovies.MoviesLibrary.Business.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, MovieDTO>().ReverseMap();
        CreateMap<Actor, ActorDTO>().ReverseMap();
        CreateMap<Producer, ProducerDTO>().ReverseMap();
        CreateMap<Character, CharacterDTO>().ReverseMap();
    }
}
