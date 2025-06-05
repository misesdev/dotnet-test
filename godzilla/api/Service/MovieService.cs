using api.Data;
using api.Models;
using api.Models.Common;

namespace api.Service;

public class MovieService : BaseService<Movie> 
{
    public MovieService(AppDbContex context) : base(context) { }

    public async Task<Response<Movie>> AddAsync(RecordMovie model) 
    {
        // if(string.IsNullOrEmpty(model.Title))
        //     return Response<Movie>.Fail("O título é obrigatório!");
        // if(model.Stock <= 0)
        //     return Response<Movie>.Fail("A quantiade em estóque é obrigatória!");
        // if(string.IsNullOrEmpty(model.Director))
        //     return Response<Movie>.Fail("O Diretor é obrigatório!");

        var movie = await base.AddAsync(new Movie {
            Title = model.Title,
            Director = model.Director,
            Stock = model.Stock,
            CoverSource = model.CoverSource,
            Source = model.Source
        });

        return Response<Movie>.Ok("", movie);
    }

    public async Task<Response<Movie>> GetById(Guid id)
    {
        var movie = await base.GetByIdAsync(id);

        if(movie == null)
            return Response<Movie>.Fail("Filme não encontrado!");

        return Response<Movie>.Ok("", movie);
    }

    public async Task<Response<Result<Movie>>> SearchByTitle(MovieSearch model) 
    {
        var lista = await base.FilterAsync(
            m => m.Title.Contains(model.SearchTerm), 
            model.Page, 
            model.ItemsPerPage
        );

        var results = new Result<Movie> {
            Page = model.Page,
            ItemsPerPage = model.ItemsPerPage,
            Items = lista ?? new List<Movie>()
        };

        return Response<Result<Movie>>.Ok("", results);
    }

}

