using Contracts;
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
        private ILoggerManager _logger;
        public DirectoryController(DataDpContext context,ILoggerManager logger)
        {
            _contaxt = context;
            _logger = logger;
        }


        [HttpGet]
        public async  Task<ActionResult<List<Directory>>> GetDirectory()
        {
            try
            {
                _logger.LogInfo("Projects.Get() has been run.");
                return Ok(await _contaxt.Directory.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Projects.Get() has been crashed:" + ex.Message);
                throw;
            }
        }
        //[HttpGet("MyDirectory")]
        //public async Task<IActionResult> GetUserDirectory()
        //{
        //    try
        //    {
        //        // Kullanıcının kimliğini alın (kimlik doğrulama sonucunda)
        //        int userId = int.Parse(User.Identity.Name);

        //        // Kullanıcının rehberini sorgulayın
        //        var userDirectory = await _contaxt.Directory
        //            .Where(d => d.UserId == userId) // User ile ilişkilendirilmiş rehberi seçin
        //            .ToListAsync();

        //        return Ok(userDirectory); // Kullanıcının rehberini gönderin
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


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
