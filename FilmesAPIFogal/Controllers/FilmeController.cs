using AutoMapper;
using FilmesAPIFogal.Data;
using FilmesAPIFogal.Data.Dtos;
using FilmesAPIFogal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPIFogal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {

        private FilmeContext _context;
        private IMapper _mapper;

        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<Filme> RecuperarFilmes()
        {
            return _context.Filmes;
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaFilmesPorId(int id)
        {
            var retornoFilme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (retornoFilme != null)
            {
                ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(retornoFilme);
                return Ok(filmeDto);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);

            _context.Filmes.Add(filme);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaFilmesPorId), new { Id = filme.Id }, filme);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaFilmePorId(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            var retornoFilme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if(retornoFilme == null)
            {
                return NotFound();
            }
            _mapper.Map(filmeDto, retornoFilme);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult ApagaFilmePorId(int id)
        {
            var retornoFilme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (retornoFilme == null)
            {
                return NotFound();
            }

            _context.Remove(retornoFilme);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
