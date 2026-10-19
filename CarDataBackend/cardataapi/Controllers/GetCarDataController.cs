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
        public IActionResult Get([FromQuery] int tpid){
            if(tpid == null)
                return BadRequest();

            TestPersonData tpd = getTestPersonRepo.GetTPD(tpid);
            if(tpd == null)
                return BadRequest();

            return Ok(tpd);
        }

    }
}
