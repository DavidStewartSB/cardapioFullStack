using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.DTOs;
using AutoMapper;
using Cardapio.API.Models;
using Cardapio.API.Pagination;
using Cardapio.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cardapio.API.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/[Controller]")]
    [ApiController]

    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork contexto, IMapper mapper)
        {
           _uof = contexto;
           _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            try
            {
                var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);
                
                var metadata = new
                {
                    produtos.TotalCount,
                    produtos.PageSize,
                    produtos.CurrentPage,
                    produtos.TotalPages,
                    produtos.HasNext,
                    produtos.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                "Erro ao tentar obter os produtos do banco de dados");
            }
           
        }

        [HttpGet("{id}", Name="ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            try
            {
            var produto =  await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            var produtoDto  = _mapper.Map<ProdutoDTO>(produto);
                if(produto == null)
                    {
                        return NotFound($"O produto com id={id}, não foi encontrado!");
                    }
                return produtoDto;
            }
            catch(Exception)
            {
                throw;
            }
           
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post([FromBody]ProdutoDTO produtoDto)
        {

            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Add(produto);
            
            await _uof.Commit();


            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return new CreatedAtRouteResult("ObterProduto", 
                   new {id = produto.ProdutoId}, produtoDTO);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                 "Erro ao tentar criar um novo Produto");
            }
            
        }

        [HttpPut("{id}")]
        public  async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            if(id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();
            return Ok("Produto Atualizado com sucesso");
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto =  await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto == null)
            {
                return NotFound($"Produto id = {id} não encontrado ");
            }
            _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
        }
        
    }

    
}