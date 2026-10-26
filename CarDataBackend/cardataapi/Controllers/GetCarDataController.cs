using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace cardataapi.Controllers{
    [Route("api/get")]
    [ApiController]
    public class GetCarDataController:ControllerBase{
        private readonly GetTestPersonData getTestPersonRepo;
        public GetCarDataController(GetTestPersonData getTestPersonRepo){
            this.getTestPersonRepo = getTestPersonRepo;
        }
        [HttpGet]
        [Authorize]
        [Route("tpid")]
        public async Task<IActionResult> Get([FromQuery] int tpid){
            if(tpid == 0)
                return BadRequest();

            List<TestPersonData>? tpds = await getTestPersonRepo.GetTPD(tpid);
            if(tpds == null)
                return BadRequest();

            return Ok(tpds);
        }

    }
}
