using Microsoft.AspNetCore.Mvc;
using CARDataLib;
namespace cardataapi.Controllers{

    //API routen er custom
    [Route("api/cardata")]
    [ApiController]
    public class CarDataSqliteController : ControllerBase
    {
       private ByteHandlerService byteHandlerService; 
       private CarDataSqliteChunkRepository carDataSqliteRepository;
       private CarDataSqliteRepository carDataSqliteSingleRepository;
       public CarDataSqliteController(ByteHandlerService byteHandlerService, CarDataSqliteRepository carDataSqliteSingleRepository, CarDataSqliteChunkRepository carDataSqliteRepository){
           this.byteHandlerService = byteHandlerService;
           this.carDataSqliteRepository = carDataSqliteRepository;
           this.carDataSqliteSingleRepository = carDataSqliteSingleRepository;
       }
       [HttpGet]
       [Route("id/{userId}")]
       public IActionResult GetUser(int userId){
           User u = carDataSqliteSingleRepository.GetUser(userId);
           if(userId == 0){
               return BadRequest();
           }
           return Ok(u);
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
    }
}
