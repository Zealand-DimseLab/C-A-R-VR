using CARDataLib;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Reflection.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cardataapi.Controllers
{
    [Route("api/cardata")]
    [ApiController]
    public class CarDataController : ControllerBase
    {
        private CarDataMssqlRepository carDataRepository;
        private CarDataChunkRepository chunkRepository;
        private ByteHandlerService byteHandlerService;
        // private CarDataSqliteRepository carDataSqliteRepository;
        public CarDataController(ByteHandlerService byteHandlerService, CarDataMssqlRepository carDataRepository, CarDataChunkRepository chunkRepository/*, CarDataSqliteRepository carDataSqliteRepository*/)
        {
            this.carDataRepository = carDataRepository;
            this.byteHandlerService = byteHandlerService;
            this.chunkRepository = chunkRepository;
            // this.carDataSqliteRepository = carDataSqliteRepository;
        }
       [HttpPost]
       [Route("logbikedata")]
       public async Task<IActionResult> PostBD(){
           using var ms = new MemoryStream();
           await Request.Body.CopyToAsync(ms);

           byte[] incomingBytes = ms.ToArray();

           if(incomingBytes.Length == 0 || incomingBytes == null) return BadRequest();
           await byteHandlerService.ByteBikeData(incomingBytes);
           return Ok("Bikedata tilføjet i db!");
       }
       [HttpPost]
       [Route("logscenario")]
       public async Task<IActionResult> PostScenarios(){
           using var ms = new MemoryStream();
           await Request.Body.CopyToAsync(ms);

           byte[] incomingBytes = ms.ToArray();

           if(incomingBytes.Length == 0 || incomingBytes == null) return BadRequest();
           await byteHandlerService.ByteScenarios(incomingBytes);
           return Ok("Scenariedata tilføjet i db!");
       }
       [HttpPost]
       [Route("loghtf")]
       public async Task<IActionResult> PostHTF(){

           using var ms = new MemoryStream();
           await Request.Body.CopyToAsync(ms);

           byte[] incomingBytes = ms.ToArray();

           if(incomingBytes.Length == 0 || incomingBytes == null) return BadRequest();
           await byteHandlerService.ByteHeadTransform(incomingBytes);
           return Ok("HeadTransformData tilføjet i db!");
       }
       [HttpPost]
       [Route("logarduino")]
       public async Task<IActionResult> PostArduino(){

           using var ms = new MemoryStream();
           await Request.Body.CopyToAsync(ms);

           byte[] incomingBytes = ms.ToArray();

           if(incomingBytes.Length == 0 || incomingBytes == null) return BadRequest();
           await byteHandlerService.ByteArduino(incomingBytes);
           return Ok("Arduinodata tilføjet i db!");
       }
       [HttpPost]
       //Endpoint
       [Route("logpulse")]
       public async Task<IActionResult> PostPulseData(){
            //Strømmen af data og hentningen af det fra Requesten
           using var ms = new MemoryStream();
           await Request.Body.CopyToAsync(ms);
            //Omsættes til byte array.
           byte[] incomingBytes = ms.ToArray();
            //Check if null or 0 
           if(incomingBytes.Length == 0 || incomingBytes == null) return BadRequest();
           //Hvis der er data så sender vi det videre til vore service lag.
           await byteHandlerService.BytePulseData(incomingBytes);
           //Hvis alt går som det skal får Unity en OK 200 tilbage.
           return Ok("Pulse data indsat.");
       }
       [HttpPost]
       [Route("quaterone")]
       public async Task<IActionResult> QuaterOne(User quaterone){
           if(quaterone == null)
               return BadRequest();
            User q1user = carDataRepository.InsertQuaterOne(quaterone); 
            return Ok(q1user);
       }
       [HttpPost]
       [Route("quatertwo")]
       public async Task<IActionResult> QuaterTwo(User quatertwo){
           if(quatertwo == null)
               return BadRequest();
            User q2user = carDataRepository.InsertQuaterTwo(quatertwo); 
            return Ok(q2user);
       }
    }
}

