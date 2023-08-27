using DirectoryApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DirectoryApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class DirectoryController : ControllerBase
    {
        private readonly DataDpContext _contaxt; 
        public DirectoryController(DataDpContext context)
        {
            _contaxt = context;
        }


        [HttpGet]
        public async  Task<ActionResult<List<Directory>>> GetDirectory()
        {
            return Ok(await _contaxt.Directory.ToListAsync());
        }

        [HttpPost]
        public async  Task<ActionResult<List<Directory>>> CreateDirectory(Directory directory)
        {
            _contaxt.Directory.Add(directory);
            await _contaxt.SaveChangesAsync();
            return Ok(await _contaxt.Directory.ToListAsync());
            
        }

        [HttpPut]
        public async Task<ActionResult<List<Directory>>> UpdateDirectory(Directory directory)
        {
            var dbDirectory = await _contaxt.Directory.FindAsync(directory.Id);
            if (dbDirectory == null)
                return BadRequest("The person you were looking for couldn't be found");

            dbDirectory.FirstName = directory.FirstName;
            dbDirectory.LastName = directory.LastName;
            dbDirectory.Telephone = directory.Telephone;

            await _contaxt.SaveChangesAsync();

            return Ok(await _contaxt.Directory.ToListAsync());

        }
            [HttpDelete("{id}")]
            public async Task<ActionResult<List<Directory>>> DeleteDirectory(int id)
            {
                var dbDirectory = await _contaxt.Directory.FindAsync(id);
                if (dbDirectory == null)
                    return BadRequest("The person you were looking for couldn't be found");

                _contaxt.Directory.Remove(dbDirectory);
                await _contaxt.SaveChangesAsync();

                return Ok(await _contaxt.Directory.ToListAsync());
            }
    }
}
