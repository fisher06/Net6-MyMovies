
using MyMovies.MoviesLibrary.Domain;
using System.Data;

namespace MyMovies.MoviesLibrary.Data.Repository;

public interface IActorRepository
{
    Task<IEnumerable<Actor>> GetAll();

    Task<Actor?> GetById(int? id);

    Task<Actor> Create(Actor? actor, IDbTransaction? transaction = null);

    Task<Actor> Update(Actor? actor, IDbTransaction? transaction = null);

    Task<int> Delete(Actor? actor, IDbTransaction? transaction = null);
}
