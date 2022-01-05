using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.DTOs;
using AutoMapper;
using Cardapio.API.Models;
using Cardapio.API.Pagination;
using Cardapio.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Cardapio.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("teste")]
        public string GetTest()
        {
            return $"CategoriasController - {DateTime.Now.ToLongDateString().ToString()}";
        }
       
       [HttpGet("produtos")]
       public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
       {
           var categoria = await _uof.CategoriaRepository.GetCategoriasProdutos();
           var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);
           return categoriaDto;
       }

        [HttpGet]
        [EnableCors("PermitirApiRequest")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriaParameters categoriaParameters)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetCategoriasPaginas(categoriaParameters);
                var metadata = new 
                {
                    categoria.TotalCount,
                    categoria.PageSize,
                    categoria.CurrentPage,
                    categoria.TotalPages,
                    categoria.HasNext,
                    categoria.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);
                return categoriaDto;
            }
            catch(System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                "Erro ao tentar obter as categorias do banco de dados");
            }

           
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _uof.CategoriaRepository
                                               .GetById(c => c.CategoriaId == id);
            
            if (categoria == null)
            {
                return NotFound($"A Categoria com id={id} não foi encontrada");
            }
            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {

            try
            {
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _uof.CategoriaRepository.Add(categoria);

            await _uof.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
                return new CreatedAtRouteResult("ObterCategoria", 
                new {id = categoria.CategoriaId}, categoriaDTO);
            }
            catch(System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  "Erro ao tentar criar uma categoria.");
            }
           
        }

        [HttpPut ("{id}")]
        public  async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            
            try
            {
                if (id != categoriaDto.CategoriaId)
                    {
                        return BadRequest($"Não foi posssivel atualizar a categoria com id= {id}");
                    }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();
                return Ok($"A Categoria com o id= {id} foi atualizada com sucesso");
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  "Erro ao tentar atualizar uma categoria");
            }
            
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            try
            {
            var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if(categoria == null)
                {
                    return NotFound($"A categoria com id= {id} não foi encontrada");
                }
                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();
                return categoria;

            }
            catch(System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                  $"Erro ao tentar excluir a categoria de id= {id}");
            }
                
        }
        

        
    }
}