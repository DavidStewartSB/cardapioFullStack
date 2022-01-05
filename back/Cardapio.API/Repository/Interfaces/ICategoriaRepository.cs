using System.Collections.Generic;
using System.Threading.Tasks;
using APiCatalogo.Repository;
using Cardapio.API.Models;
using Cardapio.API.Pagination;

namespace Cardapio.API.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategoriasPaginas(CategoriaParameters categoriaParameters);
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}