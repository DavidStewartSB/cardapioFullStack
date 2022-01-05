using System.Collections.Generic;
using System.Threading.Tasks;
using Cardapio.API.Models;
using Cardapio.API.Pagination;

namespace APiCatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutosPorPreco();
    }
}