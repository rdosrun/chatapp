using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;  // for Controller, [Route], [HttpPost], [FromBody], JsonResult and Json
using System.IO;   // for MemoryStream
using System.Net.Http; // for HttpResponseMessage
using System.Net;  // for HttpStatusCode
using System.Net.Http.Headers;  // for MediaTypeHeaderValue
namespace chatapp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }
    private String getfile(string path){
        String tmp = "";
        String results ="";
        try{
            StreamReader sr = new StreamReader(path);
            tmp = sr.ReadLine();
            while(tmp!=null){
                results = results+tmp;
                tmp = sr.ReadLine();
            }
            sr.Close();
        }
        catch(Exception e){
            //throw error page
            results = "<html> "+e.Message+" </html>";
        }
        return results;
    }

    [HttpGet("LandingPage")]
    public ContentResult Get()
    {
        return Content(getfile("./html/landingpage.html"),"text/html");
    }

    [HttpGet("LandingPage.css")]
    public async Task<ContentResult> GetCSS()
    {
        return Content(getfile("./html/landingpage.css"),"text/css");
    }

    [HttpGet("main")]
    public async Task<ContentResult> GetMain()
    {
       return Content (getfile("./html/index.html"),"text/html");
    }

    [HttpGet("mainCSS")]
    public async Task<ContentResult> GetMainCSS()
    {
       return Content (getfile("./html/index.css"),"text/css");
    }


}
