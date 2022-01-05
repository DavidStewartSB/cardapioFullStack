using Microsoft.EntityFrameworkCore.Migrations;

namespace Cardapio.API.Migrations
{
    public partial class PopulaDB : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Nome, ImagemUrl) Values('Bebidas','http:/www.macoratti.net/Imagens/1.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImagemUrl) Values('Lanches','http:/www.macoratti.net/Imagens/2.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImagemUrl) Values('Sobremesa','http:/www.macoratti.net/Imagens/3.jpg' )");

            
            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, CategoriaId) " + 
            "Values('Coca-Cola', 'Refrigerante de cola 350 ml', 5.45, 'http:/www.macoratti.net/Imagens/coca.jpg', '50',(Select CategoriaId from Categorias where Nome='Bebidas'))");
            
            mb.Sql("Insert Into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, CategoriaId) " + 
            "Values('Lanche de Atum', 'Lanche de Atum com Maionese',8.50, 'http:/www.macoratti.net/Imagens/Atum.jpg', '10', (Select CategoriaId from Categorias where Nome='Lanches'))");
            
            mb.Sql("Insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, CategoriaId) " + 
            "Values('Pudim', 'Pudim de leite condensado', 6.50, 'http:/www.macoratti.net/Imagens/pudim.jpg', 12, (Select CategoriaId from Categorias where Nome='Sobremesa'))");
           
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
            mb.Sql("Delete from Produtos");
        }
    }
}
