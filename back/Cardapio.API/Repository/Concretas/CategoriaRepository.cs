using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APiCatalogo.Repository;
using Cardapio.API.Context;
using Cardapio.API.Models;
using Cardapio.API.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Cardapio.API.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {}

        public async Task<PagedList<Categoria>> GetCategoriasPaginas(CategoriaParameters categoriaParameters)
        {
            return await PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.CategoriaId),
                                         categoriaParameters.PageNumber,
                                         categoriaParameters.PageSize);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
           return await Get().Include(x => x.Produtos).ToListAsync();
        }

        
    }
}