using System.Threading.Tasks;
using APiCatalogo.Repository;

namespace Cardapio.API.Repository
{
    public interface IUnitOfWork
    {
         IProdutoRepository ProdutoRepository { get; }
         ICategoriaRepository CategoriaRepository { get; }
         Task Commit();
    }
}