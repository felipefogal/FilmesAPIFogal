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

        public FilmeController(FilmeContext context)
        {
            _context = context;
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
                ReadFilmeDto filmeDto = new ReadFilmeDto
                {
                    Id = retornoFilme.Id,
                    Titulo = retornoFilme.Titulo,
                    Diretor = retornoFilme.Diretor,
                    Genero = retornoFilme.Genero,
                    Duracao = retornoFilme.Duracao,
                    HoraDaConsulta = DateTime.Now
                };
                return Ok(filmeDto);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = new Filme
            {
                Titulo = filmeDto.Titulo,
                Genero = filmeDto.Genero,
                Duracao = filmeDto.Duracao,
                Diretor = filmeDto.Diretor
            };

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
            retornoFilme.Titulo = filmeDto.Titulo;
            retornoFilme.Diretor = filmeDto.Diretor;
            retornoFilme.Genero = filmeDto.Genero;
            retornoFilme.Duracao = filmeDto.Duracao;
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

            _context.Filmes.Remove(retornoFilme);
            _context.SaveChanges();
            return NoContent();
        }

        //private IActionResult RetornaFilmes(int id)
        //{
        //    Filme retornoFilme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        //    if (retornoFilme == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(retornoFilme);
        //}
    }
}
