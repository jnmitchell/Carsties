using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        public AuctionsController(AuctionDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }


        [HttpGet]
        // note: no route makes it the default method

        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions.Include(x => x.Item)
                .OrderBy(x => x.Item.Make)
                .ToListAsync();

            return _mapper.Map<List<AuctionDto>>(auctions);
        }


        [HttpGet("{id}")]
        // note: id makes the default route take an id
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions.Include(x => x.Item)
                .OrderBy(x => x.Item)
              .FirstOrDefaultAsync(x => x.Id==id);

            if (auction == null) return NotFound();

            return _mapper.Map<AuctionDto>(auction);
        }

        [HttpPost]

        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            // TODO: add cur user as seller

            auction.Seller="test";
            _context.Auctions.Add(auction);
            var result = await _context.SaveChangesAsync()>0;

            if (!result) return BadRequest("Could not create auction");

            return CreatedAtAction(nameof(GetAuctionById),
                new { auction.Id }, _mapper.Map<AuctionDto>(auction));
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<AuctionDto>> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await _context.Auctions.Include(_x => _x.Item) 
                .FirstOrDefaultAsync(_x => _x.Id==id);
            if (auction == null) return NotFound();

            // TODO: add cur user as seller

            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;


            var result = await _context.SaveChangesAsync()>0;

            if (result) return Ok();

            return BadRequest("problem saving changes");
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<AuctionDto>> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions.FindAsync(id);
               
            if (auction == null) return NotFound();

            // TODO: add cur user as seller

            _context.Auctions.Remove(auction);

            var result = await _context.SaveChangesAsync()>0;

            if (result) return Ok();

            return BadRequest("problem deleting changes");
        }

    }
}