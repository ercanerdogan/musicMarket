using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicMarket.Api.DTO;
using MusicMarket.Api.Validators;
using MusicMarket.Core.Models;
using MusicMarket.Core.Services;

namespace MusicMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly IMapper _mapper;

        public ArtistController(IArtistService artistService, IMapper mapper)
        {
            _artistService = artistService;
            _mapper = mapper;

        }
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetAllArtist()
        {
            var artists = await _artistService.GetAllArtists();
            var artistResource = _mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistDTO>>(artists);

            return Ok(artistResource);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistDTO>> GetArtistById(int id)
        {
            var artist = await _artistService.GetArtistById(id);
            var artistResource = _mapper.Map<Artist, ArtistDTO>(artist);

            return Ok(artistResource);
        }

        [HttpPost("")]
        public async Task<ActionResult<ArtistDTO>> CreateArtist([FromBody] SaveArtistDTO saveArtistResource)
        {
            var validator = new SaveArtistResourceValidator();
            var validationResult = await validator.ValidateAsync(saveArtistResource);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var artistToCreate = _mapper.Map<SaveArtistDTO, Artist>(saveArtistResource);
            var newArtist = await _artistService.CreateArtist(artistToCreate);

            var artist = await _artistService.GetArtistById(newArtist.Id);
            var artistResource = _mapper.Map<Artist, ArtistDTO>(artist);

            return Ok(artistResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArtistDTO>> UpdateArtist(int id, [FromBody] SaveArtistDTO saveArtistResource)
        {
            var validator = new SaveArtistResourceValidator();
            var validationResult = await validator.ValidateAsync(saveArtistResource);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var artistToBeUpdated = await _artistService.GetArtistById(id);

            if (artistToBeUpdated == null)
            {
                return NotFound();
            }

            var artist = _mapper.Map<SaveArtistDTO, Artist>(saveArtistResource);

            await _artistService.UpdateArtist(artistToBeUpdated, artist);

            var updatedArtist = await _artistService.GetArtistById(id);
            var updatedArtistResource = _mapper.Map<Artist, ArtistDTO>(updatedArtist);

            return Ok(updatedArtistResource);


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var artist = await _artistService.GetArtistById(id);

            if (artist == null)
            {
                return NotFound();
            }

            await _artistService.DeleteArtist(artist);

            return NoContent();

        }



    }
}