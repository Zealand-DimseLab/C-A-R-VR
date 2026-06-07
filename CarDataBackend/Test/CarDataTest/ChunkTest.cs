namespace CarDataTest;
using CARDataLib;
using System.IO;
using cardataapi;
using System.Threading.Tasks;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;

[TestClass]
public sealed class ChunkTest{
    // private CarDataChunkRepository? chunkRepository;
    // private FileHandlerService fHService;
    // private CarDataSqliteChunkRepository? chunkSqliteRepository;
    // private string path = @"D:\Datamatikeruddannelse\cardataapi\cardataapi\cardb";
    //
    // // private CarDataMssqlRepository? carDataMssqlRepository;
    // [TestInitialize]
    // public void TestInitialize(){
    //     this.chunkRepository = new CarDataChunkRepository("Server=localhost;Database=CARData;Trusted_Connection=True;TrustServerCertificate=True;");
    //     // this.carDataMssqlRepository = new CarDataMssqlRepository("Server=localhost;Database=CARData;Trusted_Connection=True;TrustServerCertificate=True;");
    //     this.chunkSqliteRepository = new CarDataSqliteChunkRepository($"Data Source={path};"); 
    //     this.fHService = new FileHandlerService(chunkSqliteRepository, chunkRepository);
    // }
    // [TestMethod]
    // public void FirstInstanceOfUserTest(){
    //
    // }
    // [TestMethod]
    // public async Task InsertIntoHTFChunk()
    // {
    //     string path = @"C:\Users\nqvis\Desktop\Test.txt";
    //
    //     var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
    //
    //     IFormFile file = new FormFile( stream, 0, stream.Length, "newTestFile", Path.GetFileName(path));
    //
    //     var incomingFile = new IncomingFile { newTestFile = file };
    //     // Act
    //     await fHService.HandleHeadTrans(incomingFile);
    //     Assert.IsNotNull(path);
    // }
    // [TestMethod]
    // public async Task InsertIntoBikeDataChunk()
    // {
    //     string path = @"C:\Users\nqvis\Desktop\Test1.txt";
    //
    //     var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
    //
    //     IFormFile file = new FormFile(
    //             stream,
    //             0,
    //             stream.Length,
    //             "newTestFile",
    //             Path.GetFileName(path)
    //             );
    //
    //     var incomingFile = new IncomingFile
    //     {
    //         newTestFile = file
    //     };
    //
    //
    //     // Act
    //     await fHService.HandleBikeData(incomingFile);
    //     Assert.IsNotNull(path);
    // }
    // [TestMethod]
    // public async Task InsertIntoScenarioChunk()
    // {
    //     string path = @"C:\Users\nqvis\Desktop\Test2.txt";
    //
    //     var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
    //
    //     IFormFile file = new FormFile(
    //             stream,
    //             0,
    //             stream.Length,
    //             "newTestFile",
    //             Path.GetFileName(path)
    //             );
    //
    //     var incomingFile = new IncomingFile
    //     {
    //         newTestFile = file
    //     };
    //
    //
    //     // Act
    //     await fHService.HandleScenarios(incomingFile);
    //     Assert.IsNotNull(path);
    // }
}
