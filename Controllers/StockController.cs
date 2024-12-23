using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController: ControllerBase
    {

        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockRepo.GetAllAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById([FromRoute] int id){
            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock==null){
            return NotFound();
            }

            return Ok(stock);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){
            var stock = stockDto.ToStockFromCreateDto();

            await _stockRepo.CreateAsync(stock);

            return CreatedAtAction(nameof(GetById), new {id = stock.Id}, stock.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto){
            var stock = await _stockRepo.UpdateAsync(id, updateDto);

            if (stock == null){
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> Delete([FromRoute] int id){
            var stock =  await _stockRepo.DeleteAsync(id);

            if (stock == null){
                return NotFound();
            }

            return NoContent();
        }
    }
}