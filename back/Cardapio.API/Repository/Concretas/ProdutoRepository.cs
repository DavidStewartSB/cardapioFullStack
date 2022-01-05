using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cardapio.API.Context;
using Cardapio.API.Models;
using Cardapio.API.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APiCatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {}

        public async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters)
        {
           return await PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.Nome),
                                     produtosParameters.PageNumber,
                                     produtosParameters.PageSize);

        }
        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(c => c.Preco).ToListAsync();
        }
    }
 
}