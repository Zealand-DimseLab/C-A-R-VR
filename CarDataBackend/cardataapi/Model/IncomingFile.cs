namespace cardataapi;
using System.Text;
using Microsoft.AspNetCore.Mvc;
public class IncomingFile{

    public int Id {get; set;}
    [FromForm(Name = "newUpload")]
    public IFormFile newTestFile {get; set;}

    // public async Task<string> ReadAsList(IFormFile file){
    //     StreamReader reader = new StreamReader(file.OpenReadStream());
    //     using(reader){
    //         return await reader.ReadToEndAsync();
    //     }
    // }
}
