// using CARDataLib;
// using System.IO;
// using System.Text;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Data.SqlClient;
// using Microsoft.Data.Sqlite;
// using System.Reflection.Metadata;
//
// // For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//
// namespace cardataapi.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class CarDataController : ControllerBase
//     {
        // private CarDataMssqlRepository carDataRepository;
        // private CarDataChunkRepository chunkRepository;
        // private CarDataSqliteRepository carDataSqliteRepository;
        // public CarDataController(CarDataMssqlRepository carDataRepository, CarDataChunkRepository chunkRepository, CarDataSqliteRepository carDataSqliteRepository)
        // {
        //     this.carDataRepository = carDataRepository;
        //     this.chunkRepository = chunkRepository;
        //     this.carDataSqliteRepository = carDataSqliteRepository;
        // }
        // // GET: api/<PostController>
        // [HttpGet]
        // public IActionResult Get()
        // {
        //     List<User> users = carDataRepository.GetUsers();
        //     if(users != null)
        //         return Ok(users);
        //     else
        //         return BadRequest();
        // }
        // [HttpPost]
        // [Route("postLiteData")]
        // public IActionResult PostBikeData([FromBody] BikeData bikeData, int userId){
        //     carDataSqliteRepository.AddBikeData(bikeData, userId); 
        //     if(bikeData == null){
        //         return BadRequest();
        //     }
        //     return Created("Did good", bikeData);
        // }
        // [HttpPost]
        // [Route("logbikedata")]
        // public async Task<IActionResult> PostBikeData([FromForm] IncomingFile incomingFile, int userId){
        //     if(incomingFile.newTestFile == null || incomingFile.newTestFile.Length == 0){
        //         return BadRequest("File fail");
        //     }
        //     var folderpath = Path.Combine(Directory.GetCurrentDirectory(), "TestUsers");
        //     var filepath = Path.Combine("TestUsers", incomingFile.newTestFile.FileName);
        //
        //     Directory.CreateDirectory(folderpath);
        //     FileStream stream = new FileStream(filepath, FileMode.Create);
        //     using(stream){
        //         await incomingFile.newTestFile.CopyToAsync(stream);
        //     }
        //     string fileContent = await System.IO.File.ReadAllTextAsync(filepath);
        //     if(fileContent.Length == 0 && fileContent == null)
        //         return NoContent();
        //     string[] stringSplit = fileContent.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
        //
        //     var chunks = stringSplit.Chunk(3);
        //
        //     List<BikeData> bikeDatas = new List<BikeData>();
        //
        //     foreach (string[] chunk in chunks){
        //         if(chunk.Length < 3) {
        //             return BadRequest();
        //         };
        //             var yRot = double.Parse(chunk[0]);
        //             double curbSide = double.Parse(chunk[1]);
        //             double spee = double.Parse(chunk[2]);
        //             BikeData p = new BikeData();
        //             p.HandleRotationY = yRot;
        //             p.DistanceCurbSide = curbSide;
        //             p.Speed = spee;
        //             bikeDatas.Add(p);
        //             chunkRepository.AddBikeData(bikeDatas, userId);
        //     }
        //     return Ok("Did good!");
        // }
        // [HttpPost]
        // [Route("logpulse")]
        // public async Task<IActionResult> PostPulseData([FromForm] IncomingFile incomingFile, int userId){
        //     if(incomingFile.newTestFile == null || incomingFile.newTestFile.Length == 0){
        //         return BadRequest("File fail");
        //     }
        //     var folderpath = Path.Combine(Directory.GetCurrentDirectory(), "TestUsers");
        //     var filepath = Path.Combine("TestUsers", incomingFile.newTestFile.FileName);
        //
        //     Directory.CreateDirectory(folderpath);
        //     FileStream stream = new FileStream(filepath, FileMode.Create);
        //     using(stream){
        //        await incomingFile.newTestFile.CopyToAsync(stream);
        //     }
        //     string fileContent = await System.IO.File.ReadAllTextAsync(filepath);
        //     if(fileContent.Length == 0 && fileContent == null)
        //         return NoContent();
        //     string[] stringSplit = fileContent.Split("\n");
        //
        //     List<PulseData> newPulseList = new List<PulseData>();
        //
        //     foreach (string content in stringSplit){
        //         int pulse = int.Parse(content);
        //         PulseData p = new PulseData();
        //         p.Pulse = pulse;
        //         newPulseList.Add(p);
        //     }
        //     chunkRepository.AddPulseData(newPulseList, userId);
        //
        //     return Ok("Did good!");
        // }
        // [HttpPost]
        // [Route("userp")]
        // public IActionResult Post([FromBody] User user)
        // {
        //     if (user != null)
        //     {
        //         User testuser = carDataRepository.AddFirstInstanceOfUser(user);
        //         return Ok(testuser);
        //     }
        //     else
        //     {
        //         return BadRequest();
        //     }
        // }
        // //POST api/<PostController>
        // [HttpPost]
        // [Route("postsession")]
        // public IActionResult Post([FromBody] int userId)
        // {
        //     Session newSession = carDataRepository.AddSession(userId);
        //     if(newSession != null)
        //     {
        //         return Ok("Session startet: " + newSession.Id);
        //     }
        //     else
        //     {
        //         return BadRequest();
        //     }
        // }
        //
        // // PUT api/<PostController>/5
        // [HttpPost]
        // [Route("postscenario")]
        // public IActionResult Post([FromBody] Scenario scenario, int sessionid)
        // {
        //     Scenario newscenario = carDataRepository.AddScenario(scenario, sessionid);
        //     if(newscenario != null)
        //     {
        //         return Ok(newscenario);
        //     }
        //     else
        //     {
        //         return BadRequest();
        //     }
        // }
        // [HttpPost]
        // [Route("postpulsedata")]
        // public IActionResult Post([FromBody] PulseData pulseData, int userId)
        // {
        //     PulseData newpulseData = carDataRepository.AddPulseData(pulseData, userId);
        //     if (newpulseData != null)
        //     {
        //         return Ok(newpulseData);
        //     }
        //     else
        //     {
        //         return BadRequest();
        //     }
        // }
        // [HttpPost]
        // [Route("posttimecheck")]
        // public IActionResult Post([FromBody] TimeCheck timeCheck)
        // {
        //     TimeCheck newtimecheck = carDataRepository.AddTimeCheck(timeCheck);
        //     if (newtimecheck != null)
        //     {
        //         return Ok(newtimecheck);
        //     }
        //     else
        //     {
        //         return BadRequest();
        //     }
        // }
        // [HttpPost]
        // [Route("postbikedata")]
        // public IActionResult Post([FromBody] BikeData bikeData, int userId)
        // {
        //     BikeData newbikeDate = carDataRepository.AddBikeData(bikeData, userId);
        //     if (newbikeDate != null)
        //     {
        //         return Ok(newbikeDate);
        //     }
        //     else
        //     {
        //         return BadRequest();
        //     }
        // }
        // [HttpPost]
        // [Route("postheadtransform")]
        // public IActionResult Post([FromBody] HeadTransform headTransform, int userId)
        // {
        //     HeadTransform newheadTransform = carDataRepository.AddHeadTransform(headTransform, userId);
        //     if (headTransform != null)
        //     {
        //         return Ok(headTransform);
        //     }
        //     else
        //     {
        //         return BadRequest();
        //     }
        // }
        // [HttpPost]
        // [Route("pfs")]
        // public IActionResult Post([FromBody] IFormFile file){
        //     if(file == null || file.Length == 0) return BadRequest();
        //     //Mangler resten af indlæsningen.
        //     return Ok(file);
        // }
//     }
// }

